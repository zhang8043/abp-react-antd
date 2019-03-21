import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetAllPermissions() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Permission/GetAllPermissions",
        { method: 'GET' },
    );
}