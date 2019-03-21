import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetAllSettings() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/HostSettings/GetAllSettings",
        { method: 'GET' },
    );
}

export async function UpdateAllSettings(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/HostSettings/UpdateAllSettings",
        { method: 'PUT', body: params },

    );
}

export async function SendTestEmail(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/HostSettings/SendTestEmail",
        { method: 'POST', body: params },
    );
}
