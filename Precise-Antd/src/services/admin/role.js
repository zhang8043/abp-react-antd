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

export async function getRoleForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Role/GetRoleForEdit",
        { method: 'GET' },
        params
    );
}

export async function createOrUpdateRole(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Role/createOrUpdateRole",
        { method: 'POST' },
        params
    );
}

export async function deleteRole(params) {
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