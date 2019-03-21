import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetLanguages() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Language/GetLanguages",
        { method: 'GET' },
    );
}

export async function GetLanguageForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Language/GetLanguageForEdit",
        { method: 'GET' },
        params
    );
}

export async function CreateOrUpdateLanguage(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Language/CreateOrUpdateLanguage",
        { method: 'POST', body: params },
    );
}

export async function DeleteLanguage(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Language/DeleteLanguage",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}

export async function SetDefaultLanguage(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Language/SetDefaultLanguage",
        { method: 'POST', body: params },
    );
}

export async function GetLanguageTexts(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Language/GetLanguageTexts",
        { method: 'GET' },
        params
    );
}

export async function UpdateLanguageText(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Language/UpdateLanguageText",
        { method: 'PUT', body: params },
    );
}