import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetTenants(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/GetTenants",
        { method: 'GET' },
        params
    );
}

export async function CreateTenant(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/CreateTenant",
        { method: 'POST', body: params },
    );
}

export async function GetTenantForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/GetTenantForEdit",
        { method: 'GET' },
        params
    );
}

export async function UpdateTenant(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/UpdateTenant",
        { method: 'PUT', body: params },
    );
}

export async function DeleteTenant(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/DeleteTenant",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}

export async function GetTenantFeaturesForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/GetTenantFeaturesForEdit",
        { method: 'GET' },
        params
    );
}

export async function UpdateTenantFeatures(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/UpdateTenantFeatures",
        { method: 'PUT', body: params },
    );
}

export async function ResetTenantSpecificFeatures(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/ResetTenantSpecificFeatures",
        { method: 'POST', body: params },
    );
}

export async function UnlockTenantAdmin(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Tenant/UnlockTenantAdmin",
        { method: 'POST', body: params },
    );
}