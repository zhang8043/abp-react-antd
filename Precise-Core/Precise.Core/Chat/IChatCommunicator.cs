using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.RealTime;
using Precise.Friendships;

namespace Precise.Chat
{
    public interface IChatCommunicator
    {
        Task SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message);

        Task SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friend, bool isOwnRequest, bool isFriendOnline);

        Task SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected);

        Task SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState);

        Task SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user);

        Task SendReadStateChangeToClients(IReadOnlyList<IOnlineClient> onlineFriendClients, UserIdentifier user);
    }
}
