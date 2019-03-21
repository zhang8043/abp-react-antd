import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetEditionsForCombobox(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/CommonLookup/GetEditionsForCombobox",
        { method: 'GET' },
        params
    );
}

export async function FindUsers(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/CommonLookup/FindUsers",
        { method: 'POST', body: params }
    );
}

export async function GetDefaultEditionName() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/CommonLookup/GetDefaultEditionName",
        { method: 'GET' },
    );
}