import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetPaymentInfo(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Payment/GetPaymentInfo",
        { method: 'GET' },
        params
    );
}

export async function CreatePayment(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Payment/CreatePayment",
        { method: 'POST', body: params },
    );
}

export async function CancelPayment(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Payment/CancelPayment",
        { method: 'POST', body: params },
    );
}

export async function ExecutePayment(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Payment/ExecutePayment",
        { method: 'POST', body: params },
    );
}

export async function GetPaymentHistory(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Payment/GetPaymentHistory",
        { method: 'GET' },
        params
    );
}