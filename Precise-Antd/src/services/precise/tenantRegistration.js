import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function RegisterTenant(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantRegistration/RegisterTenant",
        { method: 'POST', body: params },
    );
}

export async function GetEditionsForSelect() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantRegistration/GetEditionsForSelect",
        { method: 'GET' },
    );
}

export async function GetEdition(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantRegistration/GetEdition",
        { method: 'GET' },
        params
    );
}