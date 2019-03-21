import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function SendAndGetDate(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/DemoUiComponents/SendAndGetDate",
        { method: 'POST', body: params }
    );
}

export async function SendAndGetDateTime(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/DemoUiComponents/SendAndGetDateTime",
        { method: 'POST', body: params }
    );
}

export async function SendAndGetDateRange(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/DemoUiComponents/SendAndGetDateRange",
        { method: 'POST', body: params }
    );
}

export async function GetCountries() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/DemoUiComponents/GetCountries",
        { method: 'GET' },
    );
}

export async function SendAndGetSelectedCountries(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/DemoUiComponents/SendAndGetSelectedCountries",
        { method: 'POST', body: params }
    );
}

export async function SendAndGetValue(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/DemoUiComponents/SendAndGetValue",
        { method: 'POST', body: params }
    );
}