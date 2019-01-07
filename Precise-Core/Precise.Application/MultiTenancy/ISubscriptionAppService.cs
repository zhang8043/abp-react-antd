using System.Threading.Tasks;
using Abp.Application.Services;

namespace Precise.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task UpgradeTenantToEquivalentEdition(int upgradeEditionId);
    }
}
