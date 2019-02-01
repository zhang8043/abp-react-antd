using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.DataItems.Dtos;

namespace Precise.DataItems
{
    public interface IItemsEntityAppService : IApplicationService
    {
        Task<PagedResultDto<ItemsEntityListDto>> GetPaged(GetItemsEntitysInput input);
        Task<ItemsEntityListDto> GetById(EntityDto<string> input);
        Task<GetItemsEntityForEditOutput> GetForEdit(EntityDto<string> input);
        Task CreateOrUpdate(CreateOrUpdateItemsEntityInput input);
        Task Delete(EntityDto<string> input);
        Task BatchDelete(List<string> input);
    }
}
