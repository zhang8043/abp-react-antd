using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Precise.WorkFlow.Dtos;

namespace Precise.WorkFlow
{
    [AbpAuthorize]
    public class FlowInstanceAppService : PreciseAppServiceBase, IFlowInstanceAppService
    {
        private readonly IRepository<FlowInstance, string> _flowInstanceRepository;

        public FlowInstanceAppService(
        IRepository<FlowInstance, string> flowInstanceRepository
            )
        {
            _flowInstanceRepository = flowInstanceRepository;
        }
        /// <summary>
        /// 获取分页列表信息
        ///</summary>
        public async Task<PagedResultDto<FlowInstanceListDto>> GetPaged(GetFlowInstancesInput input)
        {
            var query = _flowInstanceRepository.GetAll();
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<FlowInstanceListDto>>();
            return new PagedResultDto<FlowInstanceListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 通过指定id获取
        /// </summary>
        public async Task<FlowInstanceListDto> GetById(EntityDto<string> input)
        {
            var entity = await _flowInstanceRepository.GetAsync(input.Id);
            return entity.MapTo<FlowInstanceListDto>();
        }

        /// <summary>
        /// 获取编辑
        /// </summary>
        public async Task<GetFlowInstanceForEditOutput> GetForEdit(EntityDto<string> input)
        {
            var output = new GetFlowInstanceForEditOutput();
            FlowInstanceEditDto editDto;
            if (!string.IsNullOrEmpty(input.Id))
            {
                var entity = await _flowInstanceRepository.GetAsync(input.Id);
                editDto = entity.MapTo<FlowInstanceEditDto>();
            }
            else
            {
                editDto = new FlowInstanceEditDto();
            }
            output.FlowInstance = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改
        /// </summary>
        public async Task CreateOrUpdate(CreateOrUpdateFlowInstanceInput input)
        {
            if (!string.IsNullOrEmpty(input.FlowInstance.Id))
            {
                await Update(input.FlowInstance);
            }
            else
            {
                await Create(input.FlowInstance);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        protected virtual async Task<FlowInstanceEditDto> Create(FlowInstanceEditDto input)
        {
            var entity = input.MapTo<FlowInstance>();
            entity = await _flowInstanceRepository.InsertAsync(entity);
            return entity.MapTo<FlowInstanceEditDto>();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        protected virtual async Task Update(FlowInstanceEditDto input)
        {
            var entity = await _flowInstanceRepository.GetAsync(input.Id);
            input.MapTo(entity);
            await _flowInstanceRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task Delete(EntityDto<string> input)
        {
            await _flowInstanceRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public async Task BatchDelete(List<string> input)
        {
            await _flowInstanceRepository.DeleteAsync(s => input.Contains(s.Id));
        }
    }
}
