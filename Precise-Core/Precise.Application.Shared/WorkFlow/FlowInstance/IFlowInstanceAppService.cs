using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.WorkFlow.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Precise.WorkFlow
{
    /// <summary>
    /// 流程运行实例
    /// </summary>
    public interface IFlowInstanceAppService : IApplicationService
    {
        /// <summary>
        /// 获取FlowInstance的分页列表信息
        ///</summary>
        Task<PagedResultDto<FlowInstanceListDto>> GetPaged(GetFlowInstancesInput input);
        /// <summary>
        /// 通过指定id获取FlowInstanceListDto信息
        /// </summary>
        Task<FlowInstanceListDto> GetById(EntityDto<string> input);
        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        Task<GetFlowInstanceForEditOutput> GetForEdit(EntityDto<string> input);
        /// <summary>
        /// 添加或者修改FlowInstance的公共方法
        /// </summary>
        Task CreateOrUpdate(CreateOrUpdateFlowInstanceInput input);
        /// <summary>
        /// 删除FlowInstance信息的方法
        /// </summary>
        Task Delete(EntityDto<string> input);
        /// <summary>
        /// 批量删除FlowInstance
        /// </summary>
        Task BatchDelete(List<string> input);
    }
}
