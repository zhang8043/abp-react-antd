import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function Setup(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Install/Setup",
        { method: 'POST', body: params },
    );
}

export async function GetAppSettingsJson() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Install/GetAppSettingsJson",
        { method: 'GET' },

    );
}

export async function CheckDatabase() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Install/CheckDatabase",
        { method: 'POST' },
    );
}
