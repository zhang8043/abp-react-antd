using Abp.Application.Services;
using Precise.Dto;
using Precise.Logging.Dto;

namespace Precise.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
