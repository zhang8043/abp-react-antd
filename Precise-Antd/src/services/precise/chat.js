import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetUserChatFriendsWithSettings() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Chat/GetUserChatFriendsWithSettings",
        { method: 'GET' },
    );
}

export async function GetUserChatMessages(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Chat/GetUserChatMessages",
        { method: 'GET' },
        params
    );
}

export async function MarkAllUnreadMessagesOfUserAsRead(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Chat/MarkAllUnreadMessagesOfUserAsRead",
        { method: 'POST', body: params }
    );
}