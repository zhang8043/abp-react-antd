import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function CreateFriendshipRequest(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Friendship/CreateFriendshipRequest",
        { method: 'POST', body: params }
    );
}

export async function CreateFriendshipRequestByUserName(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Friendship/CreateFriendshipRequestByUserName",
        { method: 'POST', body: params }
    );
}

export async function BlockUser(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Friendship/BlockUser",
        { method: 'POST', body: params }
    );
}

export async function UnblockUser() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Friendship/UnblockUser",
        { method: 'GET' },
    );
}

export async function AcceptFriendshipRequest(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Friendship/AcceptFriendshipRequest",
        { method: 'POST', body: params }
    );
}