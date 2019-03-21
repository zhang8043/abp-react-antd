import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/GetUsers",
        { method: 'GET' },
        params
    );
}

export async function GetUsersToExcel() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/GetUsersToExcel",
        { method: 'GET' },
    );
}

export async function GetUserForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/GetUserForEdit",
        { method: 'GET' },
        params
    );
}

export async function GetUserPermissionsForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/GetUserPermissionsForEdit",
        { method: 'GET' },
        params
    );
}

export async function ResetUserSpecificPermissions(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/ResetUserSpecificPermissions",
        { method: 'POST', body: params },
    );
}

export async function UpdateUserPermissions(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/UpdateUserPermissions",
        { method: 'PUT', body: params },
    );
}

export async function CreateOrUpdateUser(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/CreateOrUpdateUser",
        { method: 'POST', body: params },
    );
}

export async function DeleteUser(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/DeleteUser",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}

export async function UnlockUser(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/User/UnlockUser",
        { method: 'POST', body: params },
    );
}