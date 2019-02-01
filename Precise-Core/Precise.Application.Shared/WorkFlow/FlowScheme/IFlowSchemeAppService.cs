using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.WorkFlow.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Precise.WorkFlow
{
    public interface IFlowSchemeAppService : IApplicationService
    {
        /// <summary>
		/// 获取FlowScheme的分页列表信息
		///</summary>
        Task<PagedResultDto<FlowSchemeListDto>> GetPaged(GetFlowSchemesInput input);
        /// <summary>
        /// 通过指定id获取FlowSchemeListDto信息
        /// </summary>
        Task<FlowSchemeListDto> GetById(EntityDto<string> input);
        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        Task<GetFlowSchemeForEditOutput> GetForEdit(EntityDto<string> input);
        /// <summary>
        /// 添加或者修改FlowScheme的公共方法
        /// </summary>
        Task CreateOrUpdate(CreateOrUpdateFlowSchemeInput input);
        /// <summary>
        /// 删除FlowScheme信息的方法
        /// </summary>
        Task Delete(EntityDto<string> input);
        /// <summary>
        /// 批量删除FlowScheme
        /// </summary>
        Task BatchDelete(List<string> input);
    }
}
