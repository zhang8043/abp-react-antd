import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function LinkToUser(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UserLink/LinkToUser",
        { method: 'POST', body: params },
    );
}

export async function GetLinkedUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UserLink/GetLinkedUsers",
        { method: 'GET' },
        params
    );
}

export async function GetRecentlyUsedLinkedUsers() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UserLink/GetRecentlyUsedLinkedUsers",
        { method: 'GET' },
    );
}

export async function UnlinkUser(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UserLink/UnlinkUser",
        { method: 'POST', body: params },
    );
}