import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function getOrganizationUnits() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/GetOrganizationUnits",
        { method: 'GET' }
    );
}

export async function getOrganizationUnitUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/GetOrganizationUnitUsers",
        { method: 'GET' }, params
    );
}

export async function createOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/CreateOrganizationUnit",
        { method: 'POST', body: params });
}

export async function updateOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/UpdateOrganizationUnit",
        { method: 'PUT', body: params });
}

export async function moveOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/MoveOrganizationUnit",
        { method: 'POST', body: params });
}

export async function deleteOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/DeleteOrganizationUnit",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params);
}

export async function removeUserFromOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/RemoveUserFromOrganizationUnit",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            },
        },
        params);
}

export async function addUsersToOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/AddUsersToOrganizationUnit",
        { method: 'POST', body: params });
}

export async function findUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/FindUsers",
        { method: 'POST', body: params });
}