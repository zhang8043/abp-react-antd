import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetTimezones(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Timing/GetTimezones",
        { method: 'GET' },
        params
    );
}

export async function GetTimezoneComboboxItems(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Timing/GetTimezoneComboboxItems",
        { method: 'GET' },
        params
    );
}