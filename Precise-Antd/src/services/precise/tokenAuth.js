import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function Authenticate(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/Authenticate",
        { method: 'POST', body: params },
    );
}

export async function LogOut() {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/LogOut",
        { method: 'GET' },
    );
}

export async function SendTwoFactorAuthCode(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/SendTwoFactorAuthCode",
        { method: 'POST', body: params },
    );
}

export async function ImpersonatedAuthenticate(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/ImpersonatedAuthenticate",
        { method: 'POST', body: params },
    );
}

export async function LinkedAccountAuthenticate(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/LinkedAccountAuthenticate",
        { method: 'POST', body: params },
    );
}

export async function GetExternalAuthenticationProviders() {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/GetExternalAuthenticationProviders",
        { method: 'GET' },
    );
}

export async function ExternalAuthenticate(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/ExternalAuthenticate",
        { method: 'POST', body: params },
    );
}

export async function TestNotification(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/TokenAuth/TestNotification",
        { method: 'GET' },
        params
    );
}