import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetLatestWebLogs() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/WebLog/GetLatestWebLogs",
        { method: 'GET' },
    );
}

export async function DownloadWebLogs() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/WebLog/DownloadWebLogs",
        { method: 'POST' },
    );
}