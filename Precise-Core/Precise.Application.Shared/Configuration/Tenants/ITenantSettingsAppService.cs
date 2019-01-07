using System.Threading.Tasks;
using Abp.Application.Services;
using Precise.Configuration.Tenants.Dto;

namespace Precise.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
