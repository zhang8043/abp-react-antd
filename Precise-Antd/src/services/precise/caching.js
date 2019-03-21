import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetAllCaches() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Caching/GetAllCaches",
        { method: 'GET' },
    );
}

export async function ClearCache(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Caching/ClearCache",
        { method: 'POST', body: params }
    );
}

export async function ClearAllCaches(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Caching/ClearAllCaches",
        { method: 'POST', body: params }
    );
}