import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetCurrentUserProfileForEdit() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/GetCurrentUserProfileForEdit",
        { method: 'GET' },
    );
}

export async function UpdateGoogleAuthenticatorKey() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/UpdateGoogleAuthenticatorKey",
        { method: 'PUT' },
    );
}

export async function SendVerificationSms() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/SendVerificationSms",
        { method: 'POST' },
    );
}

export async function VerifySmsCode(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/VerifySmsCode",
        { method: 'POST', body: params },
    );
}

export async function PrepareCollectedData() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/PrepareCollectedData",
        { method: 'POST' },
    );
}

export async function UpdateCurrentUserProfile(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/UpdateCurrentUserProfile",
        { method: 'PUT', body: params },
    );
}

export async function ChangePassword(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/ChangePassword",
        { method: 'POST', body: params },
    );
}

export async function UpdateProfilePicture(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/UpdateProfilePicture",
        { method: 'PUT', body: params },
    );
}

export async function GetPasswordComplexitySetting() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/GetPasswordComplexitySetting",
        { method: 'GET' },
    );
}

export async function GetProfilePicture() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/GetProfilePicture",
        { method: 'GET' },
    );
}

export async function GetFriendProfilePictureById(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/GetFriendProfilePictureById",
        { method: 'GET' },
        params
    );
}

export async function GetProfilePictureById(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/GetProfilePictureById",
        { method: 'GET' },
        params
    );
}

export async function ChangeLanguage(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Profile/ChangeLanguage",
        { method: 'POST', body: params },
    );
}
