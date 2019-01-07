using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.RealTime;
using Abp.UI;
using Precise.Authorization.Users;
using Precise.Friendships;
using Precise.Friendships.Cache;

namespace Precise.Chat
{
    [AbpAuthorize]
    public class ChatMessageManager : PreciseDomainServiceBase, IChatMessageManager
    {
        private readonly IFriendshipManager _friendshipManager;
        private readonly IChatCommunicator _chatCommunicator;
        private readonly IOnlineClientManager<ChatChannel> _onlineClientManager;
        private readonly UserManager _userManager;
        private readonly ITenantCache _tenantCache;
        private readonly IUserFriendsCache _userFriendsCache;
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        private readonly IChatFeatureChecker _chatFeatureChecker;

        public ChatMessageManager(
            IFriendshipManager friendshipManager,
            IChatCommunicator chatCommunicator,
            IOnlineClientManager<ChatChannel> onlineClientManager,
            UserManager userManager,
            ITenantCache tenantCache,
            IUserFriendsCache userFriendsCache,
            IUserEmailer userEmailer,
            IRepository<ChatMessage, long> chatMessageRepository,
            IChatFeatureChecker chatFeatureChecker)
        {
            _friendshipManager = friendshipManager;
            _chatCommunicator = chatCommunicator;
            _onlineClientManager = onlineClientManager;
            _userManager = userManager;
            _tenantCache = tenantCache;
            _userFriendsCache = userFriendsCache;
            _userEmailer = userEmailer;
            _chatMessageRepository = chatMessageRepository;
            _chatFeatureChecker = chatFeatureChecker;
        }

        public async Task SendMessageAsync(UserIdentifier sender, UserIdentifier receiver, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {
            CheckReceiverExists(receiver);

            _chatFeatureChecker.CheckChatFeatures(sender.TenantId, receiver.TenantId);

            var friendshipState = (await _friendshipManager.GetFriendshipOrNullAsync(sender, receiver))?.State;
            if (friendshipState == FriendshipState.Blocked)
            {
                throw new UserFriendlyException(L("UserIsBlocked"));
            }

            var sharedMessageId = Guid.NewGuid();

            await HandleSenderToReceiverAsync(sender, receiver, message, sharedMessageId);
            await HandleReceiverToSenderAsync(sender, receiver, message, sharedMessageId);
            await HandleSenderUserInfoChangeAsync(sender, receiver, senderTenancyName, senderUserName, senderProfilePictureId);
        }

        private void CheckReceiverExists(UserIdentifier receiver)
        {
            var receiverUser = _userManager.GetUserOrNull(receiver);
            if (receiverUser == null)
            {
                throw new UserFriendlyException(L("TargetUserNotFoundProbablyDeleted"));
            }
        }

        [UnitOfWork]
        public virtual long Save(ChatMessage message)
        {
            using (CurrentUnitOfWork.SetTenantId(message.TenantId))
            {
                return _chatMessageRepository.InsertAndGetId(message);
            }
        }

        [UnitOfWork]
        public virtual int GetUnreadMessageCount(UserIdentifier sender, UserIdentifier receiver)
        {
            using (CurrentUnitOfWork.SetTenantId(receiver.TenantId))
            {
                return _chatMessageRepository.Count(cm => cm.UserId == receiver.UserId &&
                                                          cm.TargetUserId == sender.UserId &&
                                                          cm.TargetTenantId == sender.TenantId &&
                                                          cm.ReadState == ChatMessageReadState.Unread);
            }
        }

        public async Task<ChatMessage> FindMessageAsync(int id, long userId)
        {
            return await _chatMessageRepository.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        }

        private async Task HandleSenderToReceiverAsync(UserIdentifier senderIdentifier, UserIdentifier receiverIdentifier, string message, Guid sharedMessageId)
        {
            var friendshipState = (await _friendshipManager.GetFriendshipOrNullAsync(senderIdentifier, receiverIdentifier))?.State;
            if (friendshipState == null)
            {
                friendshipState = FriendshipState.Accepted;

                var receiverTenancyName = receiverIdentifier.TenantId.HasValue
                    ? _tenantCache.Get(receiverIdentifier.TenantId.Value).TenancyName
                    : null;

                var receiverUser = _userManager.GetUser(receiverIdentifier);
                await _friendshipManager.CreateFriendshipAsync(
                    new Friendship(
                        senderIdentifier,
                        receiverIdentifier,
                        receiverTenancyName,
                        receiverUser.UserName,
                        receiverUser.ProfilePictureId,
                        friendshipState.Value)
                );
            }

            if (friendshipState.Value == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessage(
                senderIdentifier,
                receiverIdentifier,
                ChatSide.Sender,
                message,
                ChatMessageReadState.Read,
                sharedMessageId,
                ChatMessageReadState.Unread
            );

            Save(sentMessage);

            await _chatCommunicator.SendMessageToClient(
                _onlineClientManager.GetAllByUserId(senderIdentifier),
                sentMessage
                );
        }

        private async Task HandleReceiverToSenderAsync(UserIdentifier senderIdentifier, UserIdentifier receiverIdentifier, string message, Guid sharedMessageId)
        {
            var friendshipState = (await _friendshipManager.GetFriendshipOrNullAsync(receiverIdentifier, senderIdentifier))?.State;

            if (friendshipState == null)
            {
                var senderTenancyName = senderIdentifier.TenantId.HasValue ?
                    _tenantCache.Get(senderIdentifier.TenantId.Value).TenancyName :
                    null;

                var senderUser = _userManager.GetUser(senderIdentifier);
                await _friendshipManager.CreateFriendshipAsync(
                    new Friendship(
                        receiverIdentifier,
                        senderIdentifier,
                        senderTenancyName,
                        senderUser.UserName,
                        senderUser.ProfilePictureId,
                        FriendshipState.Accepted
                    )
                );
            }

            if (friendshipState == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessage(
                    receiverIdentifier,
                    senderIdentifier,
                    ChatSide.Receiver,
                    message,
                    ChatMessageReadState.Unread,
                    sharedMessageId,
                    ChatMessageReadState.Read
                );

            Save(sentMessage);

            var clients = _onlineClientManager.GetAllByUserId(receiverIdentifier);
            if (clients.Any())
            {
                await _chatCommunicator.SendMessageToClient(clients, sentMessage);
            }
            else if (GetUnreadMessageCount(senderIdentifier, receiverIdentifier) == 1)
            {
                var senderTenancyName = senderIdentifier.TenantId.HasValue ?
                    _tenantCache.Get(senderIdentifier.TenantId.Value).TenancyName :
                    null;

                _userEmailer.TryToSendChatMessageMail(
                    _userManager.GetUser(receiverIdentifier),
                    _userManager.GetUser(senderIdentifier).UserName,
                    senderTenancyName,
                    sentMessage
                );
            }
        }

        private async Task HandleSenderUserInfoChangeAsync(UserIdentifier sender, UserIdentifier receiver, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {
            var receiverCacheItem = _userFriendsCache.GetCacheItemOrNull(receiver);

            var senderAsFriend = receiverCacheItem?.Friends.FirstOrDefault(f => f.FriendTenantId == sender.TenantId && f.FriendUserId == sender.UserId);
            if (senderAsFriend == null)
            {
                return;
            }

            if (senderAsFriend.FriendTenancyName == senderTenancyName &&
                senderAsFriend.FriendUserName == senderUserName &&
                senderAsFriend.FriendProfilePictureId == senderProfilePictureId)
            {
                return;
            }

            var friendship = (await _friendshipManager.GetFriendshipOrNullAsync(receiver, sender));
            if (friendship == null)
            {
                return;
            }

            friendship.FriendTenancyName = senderTenancyName;
            friendship.FriendUserName = senderUserName;
            friendship.FriendProfilePictureId = senderProfilePictureId;

            await _friendshipManager.UpdateFriendshipAsync(friendship);
        }
    }
}