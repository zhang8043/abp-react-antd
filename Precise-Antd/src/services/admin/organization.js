import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function getOrganizationUnits() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/GetOrganizationUnits",
        { method: 'GET' }
    );
}

export async function moveOrganizationUnit(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/OrganizationUnit/MoveOrganizationUnit",
        { method: 'POST', body: params });
}