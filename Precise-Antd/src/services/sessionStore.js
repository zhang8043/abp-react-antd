import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetAll() {
    return request(appConsts.remoteServiceBaseUrl + 'AbpUserConfiguration/GetAll', {
        method: 'GET',
    });
}

export async function getCurrentLoginInformation() {
    return request(appConsts.remoteServiceBaseUrl + 'api/services/app/Session/GetCurrentLoginInformations', {
        method: 'GET',
    });
}

export async function getUsessr() {
    return request(appConsts.remoteServiceBaseUrl + 'api/services/app/User/GetUsers?MaxResultCount=111&SkipCount=0', {
        method: 'GET',
    });
}