using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.WorkFlow.Dtos;

namespace Precise.WorkFlow
{
    public interface IFlowInstanceTransitionHistoryAppService : IApplicationService
    {
        /// <summary>
		/// 获取FlowInstanceTransitionHistory的分页列表信息
		///</summary>
        Task<PagedResultDto<FlowInstanceTransitionHistoryListDto>> GetPaged(GetFlowInstanceTransitionHistorysInput input);
        /// <summary>
        /// 通过指定id获取FlowInstanceTransitionHistoryListDto信息
        /// </summary>
        Task<FlowInstanceTransitionHistoryListDto> GetById(EntityDto<string> input);
        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        Task<GetFlowInstanceTransitionHistoryForEditOutput> GetForEdit(EntityDto<string> input);
        /// <summary>
        /// 添加或者修改FlowInstanceTransitionHistory的公共方法
        /// </summary>
        Task CreateOrUpdate(CreateOrUpdateFlowInstanceTransitionHistoryInput input);
        /// <summary>
        /// 删除FlowInstanceTransitionHistory信息的方法
        /// </summary>
        Task Delete(EntityDto<string> input);
        /// <summary>
        /// 批量删除FlowInstanceTransitionHistory
        /// </summary>
        Task BatchDelete(List<string> input);
    }
}
