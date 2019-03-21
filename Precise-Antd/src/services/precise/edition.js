import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetEditions() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Edition/GetEditions",
        { method: 'GET' }
    );
}

export async function GetEditionForEdit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Edition/GetEditionForEdit",
        { method: 'GET' },
        params
    );
}

export async function CreateOrUpdateEdition(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Edition/CreateOrUpdateEdition",
        { method: 'POST', body: params }
    );
}


export async function DeleteEdition(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Edition/DeleteEdition",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}

export async function GetEditionComboboxItems(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Edition/GetEditionComboboxItems",
        { method: 'GET' },
        params
    );
}