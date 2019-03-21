import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function isTenantAvailable(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/IsTenantAvailable",
        { method: 'POST', body: params }
    );
}

export async function ResolveTenantId(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/ResolveTenantId",
        { method: 'POST', body: params }
    );
}

export async function Register(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/Register",
        { method: 'POST', body: params }
    );
}

export async function SendPasswordResetCode(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/SendPasswordResetCode",
        { method: 'POST', body: params }
    );
}

export async function ResetPassword(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/ResetPassword",
        { method: 'POST', body: params }
    );
}

export async function SendEmailActivationLink(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/SendEmailActivationLink",
        { method: 'POST', body: params }
    );
}

export async function ActivateEmail(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/ActivateEmail",
        { method: 'POST', body: params }
    );
}

export async function Impersonate(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/Impersonate",
        { method: 'POST', body: params }
    );
}

export async function BackToImpersonator() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/BackToImpersonator",
        { method: 'POST' },
    );
}

export async function SwitchToLinkedAccount(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Account/SwitchToLinkedAccount",
        { method: 'POST', body: params }
    );
}

