import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function getRoles(permission) {
    let urls = appConsts.remoteServiceBaseUrl + "api/services/app/Role/GetRoles";
    if (permission) {
        urls += "?Permission=" + permission;
    }
    return request(urls,
        { method: 'GET' }
    );
}