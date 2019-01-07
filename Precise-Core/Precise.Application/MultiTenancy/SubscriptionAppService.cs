using System;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Precise.Editions;

namespace Precise.MultiTenancy
{
    public class SubscriptionAppService : PreciseAppServiceBase, ISubscriptionAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;

        public SubscriptionAppService(
            TenantManager tenantManager,
            EditionManager editionManager)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
        }

        public async Task UpgradeTenantToEquivalentEdition(int upgradeEditionId)
        {
            if (await UpgradeIsFree(upgradeEditionId))
            {
                await _tenantManager.UpdateTenantAsync(
                    AbpSession.GetTenantId(), true, false, null,
                    upgradeEditionId,
                    EditionPaymentType.Upgrade
                );
            }
        }

        private async Task<bool> UpgradeIsFree(int upgradeEditionId)
        {
            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (!tenant.EditionId.HasValue)
            {
                throw new Exception("Tenant must be assigned to an Edition in order to upgrade !");
            }

            var currentEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(tenant.EditionId.Value);
            var targetEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(upgradeEditionId);
            var bothEditionsAreFree = targetEdition.IsFree && currentEdition.IsFree;
            var bothEditionsHasSamePrice = currentEdition.HasSamePrice(targetEdition);
            return bothEditionsAreFree || bothEditionsHasSamePrice;
        }
    }
}