import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetAllSettings() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantSettings/GetAllSettings",
        { method: 'GET' },
    );
}

export async function UpdateAllSettings(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantSettings/UpdateAllSettings",
        { method: 'PUT', body: params },
    );
}

export async function ClearCustomCss() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantSettings/ClearCustomCss",
        { method: 'POST' },
    );
}

export async function ClearLogo() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantSettings/ClearLogo",
        { method: 'POST' },
    );
}

export async function SendTestEmail(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantSettings/SendTestEmail",
        { method: 'POST', body: params },

    );
}