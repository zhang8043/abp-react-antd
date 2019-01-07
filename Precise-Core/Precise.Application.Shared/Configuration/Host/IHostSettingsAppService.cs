using System.Threading.Tasks;
using Abp.Application.Services;
using Precise.Configuration.Host.Dto;

namespace Precise.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
