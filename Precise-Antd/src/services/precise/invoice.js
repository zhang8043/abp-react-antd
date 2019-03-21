import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetInvoiceInfo(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Invoice/GetInvoiceInfo",
        { method: 'GET' },
        params
    );
}

export async function CreateInvoice(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Invoice/CreateInvoice",
        { method: 'POST', body: params },
    );
}
