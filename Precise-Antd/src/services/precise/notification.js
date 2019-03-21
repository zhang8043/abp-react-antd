import request from '@/utils/request';
import appConsts from '@/utils/appconst'

export async function GetUserNotifications(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Notification/GetUserNotifications",
        { method: 'GET' },
        params
    );
}

export async function SetNotificationAsRead(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Notification/SetNotificationAsRead",
        { method: 'POST', body: params },
    );
}

export async function SetAllNotificationsAsRead(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Notification/SetAllNotificationsAsRead",
        { method: 'POST', body: params },
    );
}

export async function GetNotificationSettings() {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Notification/GetNotificationSettings",
        { method: 'GET' },
    );
}

export async function UpdateNotificationSettings(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Notification/UpdateNotificationSettings",
        { method: 'PUT', body: params },
    );
}

export async function DeleteNotification(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Notification/DeleteNotification",
        {
            method: 'DELETE',
            body: {
                method: 'delete',
            }
        },
        params
    );
}
