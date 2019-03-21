import request from '@/utils/request';
import appConsts from '@/utils/appconst';

export async function GetCurrentLoginInformations() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Session/GetCurrentLoginInformations",
        { method: 'GET' },
    );
}

export async function UpdateUserSignInToken() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/UpdateUserSignInToken",
        { method: 'PUT' },
    );
}