import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetDashboardStatisticsData(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/HostDashboard/GetDashboardStatisticsData",
        { method: 'POST' },
        params
    );
}

export async function GetIncomeStatistics(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/HostDashboard/GetIncomeStatistics",
        { method: 'POST' },
        params
    );
}

export async function GetEditionTenantStatistics(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/HostDashboard/GetEditionTenantStatistics",
        { method: 'POST' },
        params
    );
}
