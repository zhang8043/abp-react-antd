import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function getAuditLogs(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetAuditLogs",
        { method: 'GET' },
        params
    );
}

export async function getAuditLogsToExcel(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetAuditLogsToExcel",
        { method: 'GET' },
        params
    );
}

export async function getEntityHistoryObjectTypes() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetEntityHistoryObjectTypes",
        { method: 'GET' },
    );
}

export async function getEntityChanges(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetEntityChanges",
        { method: 'GET' },
        params
    );
}

export async function getEntityTypeChanges(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetEntityTypeChanges",
        { method: 'GET' },
        params
    );
}

export async function getEntityChangesToExcel(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetEntityChangesToExcel",
        { method: 'GET' },
        params
    );
}

export async function getEntityPropertyChanges(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/AuditLog/GetEntityPropertyChanges",
        { method: 'GET' },
        params
    );
}