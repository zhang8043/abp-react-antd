import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetOrganizationUnits() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/GetOrganizationUnits",
        { method: 'GET' },
    );
}

export async function GetOrganizationUnitUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/GetOrganizationUnitUsers",
        { method: 'GET' },
        params
    );
}

export async function CreateOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/CreateOrganizationUnit",
        { method: 'POST', body: params },
    );
}

export async function UpdateOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/UpdateOrganizationUnit",
        { method: 'PUT', body: params },
    );
}

export async function MoveOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/MoveOrganizationUnit",
        { method: 'POST', body: params },
    );
}

export async function DeleteNotification(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/DeleteNotification",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}

export async function RemoveUserFromOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/RemoveUserFromOrganizationUnit",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}

export async function AddUsersToOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/AddUsersToOrganizationUnit",
        { method: 'POST', body: params },
    );
}

export async function FindUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/FindUsers",
        { method: 'POST', body: params },
    );
}