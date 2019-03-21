import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetRecentUserLoginAttempts() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UserLogin/GetRecentUserLoginAttempts",
        { method: 'GET' },
    );
}