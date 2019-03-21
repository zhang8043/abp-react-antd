import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetMemberActivity() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantDashboard/GetMemberActivity",
        { method: 'GET' },
    );
}

export async function GetDashboardData(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantDashboard/GetDashboardData",
        { method: 'GET' },
        params
    );
}

export async function GetSalesSummary(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantDashboard/GetSalesSummary",
        { method: 'GET' },
        params
    );
}

export async function GetRegionalStats(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantDashboard/GetRegionalStats",
        { method: 'GET' },
        params
    );
}


export async function GetGeneralStats(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/TenantDashboard/GetGeneralStats",
        { method: 'GET' },
        params
    );
}
