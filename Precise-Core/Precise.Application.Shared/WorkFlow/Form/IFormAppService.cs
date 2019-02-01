using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.WorkFlow.Dtos;

namespace Precise.WorkFlow
{
    public interface IFormAppService : IApplicationService
    {
        /// <summary>
		/// 获取Form的分页列表信息
		///</summary>
        Task<PagedResultDto<FormListDto>> GetPaged(GetFormsInput input);
        /// <summary>
        /// 通过指定id获取FormListDto信息
        /// </summary>
        Task<FormListDto> GetById(EntityDto<string> input);
        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        Task<GetFormForEditOutput> GetForEdit(EntityDto<string> input);
        /// <summary>
        /// 添加或者修改Form的公共方法
        /// </summary>
        Task CreateOrUpdate(CreateOrUpdateFormInput input);
        /// <summary>
        /// 删除Form信息的方法
        /// </summary>
        Task Delete(EntityDto<string> input);
        /// <summary>
        /// 批量删除Form
        /// </summary>
        Task BatchDelete(List<string> input);
    }
}
