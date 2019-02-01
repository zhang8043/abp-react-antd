using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.DataItems.Dtos;

namespace Precise.DataItems
{
    public interface IItemsDetailEntityAppService : IApplicationService
    {
        /// <summary>
		/// 获取ItemsDetailEntity的分页列表信息
		///</summary>
        Task<PagedResultDto<ItemsDetailEntityListDto>> GetPaged(GetItemsDetailEntitysInput input);

        /// <summary>
        /// 通过指定id获取ItemsDetailEntityListDto信息
        /// </summary>
        Task<ItemsDetailEntityListDto> GetById(EntityDto<string> input);

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        Task<GetItemsDetailEntityForEditOutput> GetForEdit(EntityDto<string> input);

        /// <summary>
        /// 添加或者修改ItemsDetailEntity的公共方法
        /// </summary>
        Task CreateOrUpdate(CreateOrUpdateItemsDetailEntityInput input);

        /// <summary>
        /// 删除ItemsDetailEntity信息的方法
        /// </summary>
        Task Delete(EntityDto<string> input);

        /// <summary>
        /// 批量删除ItemsDetailEntity
        /// </summary>
        Task BatchDelete(List<string> input);
    }
}
