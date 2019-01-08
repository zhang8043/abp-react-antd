import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function getUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/GetUsers",
        { method: 'GET' },
        params
    );
}