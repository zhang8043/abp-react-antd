import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetRoles(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Role/GetRoles",
        { method: 'GET' },
        params
    );
}

export async function GetRoleForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Role/GetRoleForEdit",
        { method: 'GET' },
        params
    );
}

export async function DeleteRole(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Role/DeleteRole",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}

export async function CreateOrUpdateRole(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Role/CreateOrUpdateRole",
        { method: 'POST', body: params },
    );
}