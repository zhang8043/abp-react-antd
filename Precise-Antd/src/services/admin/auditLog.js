import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function getAuditLogs(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetAuditLogs",
        { method: 'GET' },
        params
    );
}