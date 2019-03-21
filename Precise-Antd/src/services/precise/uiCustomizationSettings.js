import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetUiManagementSettings() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UiCustomizationSettings/GetUiManagementSettings",
        { method: 'GET' },
    );
}

export async function UpdateUiManagementSettings(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UiCustomizationSettings/UpdateUiManagementSettings",
        { method: 'PUT', body: params },
    );
}

export async function UpdateDefaultUiManagementSettings(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UiCustomizationSettings/UpdateDefaultUiManagementSettings",
        { method: 'PUT', body: params },
    );
}

export async function UseSystemDefaultSettings(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/UiCustomizationSettings/UseSystemDefaultSettings",
        { method: 'POST', body: params },
    );
}
