using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.WorkFlow.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Precise.WorkFlow
{
    public interface IFlowInstanceOperationHistoryAppService : IApplicationService
    {
        /// <summary>
		/// 获取FlowInstanceOperationHistory的分页列表信息
		///</summary>
        Task<PagedResultDto<FlowInstanceOperationHistoryListDto>> GetPaged(GetFlowInstanceOperationHistorysInput input);

        /// <summary>
        /// 通过指定id获取FlowInstanceOperationHistoryListDto信息
        /// </summary>
        Task<FlowInstanceOperationHistoryListDto> GetById(EntityDto<string> input);

        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        Task<GetFlowInstanceOperationHistoryForEditOutput> GetForEdit(EntityDto<string> input);

        /// <summary>
        /// 添加或者修改FlowInstanceOperationHistory的公共方法
        /// </summary>
        Task CreateOrUpdate(CreateOrUpdateFlowInstanceOperationHistoryInput input);

        /// <summary>
        /// 删除FlowInstanceOperationHistory信息的方法
        /// </summary>
        Task Delete(EntityDto<string> input);

        /// <summary>
        /// 批量删除FlowInstanceOperationHistory
        /// </summary>
        Task BatchDelete(List<string> input);
    }
}
