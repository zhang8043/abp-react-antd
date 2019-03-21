import request from '@/utils/request';
import appConsts from '@/utils/appconst';

export async function UpgradeTenantToEquivalentEdition(params) {
    return request(appConsts.remoteServiceBaseUrl + "api/services/app/Subscription/UpgradeTenantToEquivalentEdition",
        { method: 'GET' },
        params
    );
}
