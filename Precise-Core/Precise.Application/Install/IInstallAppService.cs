using System.Threading.Tasks;
using Abp.Application.Services;
using Precise.Install.Dto;

namespace Precise.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}