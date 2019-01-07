using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.Common.Dto;
using Precise.Editions.Dto;

namespace Precise.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}