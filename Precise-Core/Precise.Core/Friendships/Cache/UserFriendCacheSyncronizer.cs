using Abp;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.ObjectMapping;
using Precise.Chat;

namespace Precise.Friendships.Cache
{
    public class UserFriendCacheSyncronizer :
        IEventHandler<EntityCreatedEventData<Friendship>>,
        IEventHandler<EntityDeletedEventData<Friendship>>,
        IEventHandler<EntityUpdatedEventData<Friendship>>,
        IEventHandler<EntityCreatedEventData<ChatMessage>>,
        ITransientDependency
    {
        private readonly IUserFriendsCache _userFriendsCache;
        private readonly IObjectMapper _objectMapper;

        public UserFriendCacheSyncronizer(
            IUserFriendsCache userFriendsCache,
            IObjectMapper objectMapper)
        {
            _userFriendsCache = userFriendsCache;
            _objectMapper = objectMapper;
        }

        public void HandleEvent(EntityCreatedEventData<Friendship> eventData)
        {
            _userFriendsCache.AddFriend(
                eventData.Entity.ToUserIdentifier(),
                _objectMapper.Map<FriendCacheItem>(eventData.Entity)
                );
        }

        public void HandleEvent(EntityDeletedEventData<Friendship> eventData)
        {
            _userFriendsCache.RemoveFriend(
                eventData.Entity.ToUserIdentifier(),
                _objectMapper.Map<FriendCacheItem>(eventData.Entity)
            );
        }

        public void HandleEvent(EntityUpdatedEventData<Friendship> eventData)
        {
            var friendCacheItem = _objectMapper.Map<FriendCacheItem>(eventData.Entity);
            _userFriendsCache.UpdateFriend(eventData.Entity.ToUserIdentifier(), friendCacheItem);
        }

        public void HandleEvent(EntityCreatedEventData<ChatMessage> eventData)
        {
            var message = eventData.Entity;
            if (message.ReadState == ChatMessageReadState.Unread)
            {
                _userFriendsCache.IncreaseUnreadMessageCount(
                    new UserIdentifier(message.TenantId, message.UserId),
                    new UserIdentifier(message.TargetTenantId, message.TargetUserId),
                    1
                );
            }
        }
    }
}
