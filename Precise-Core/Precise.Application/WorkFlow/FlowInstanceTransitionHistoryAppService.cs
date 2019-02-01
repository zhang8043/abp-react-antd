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
    public class FlowInstanceTransitionHistoryAppService : PreciseAppServiceBase, IFlowInstanceTransitionHistoryAppService
    {
        private readonly IRepository<FlowInstanceTransitionHistory, string> _flowInstanceTransitionHistoryRepository;

        public FlowInstanceTransitionHistoryAppService(
        IRepository<FlowInstanceTransitionHistory, string> flowInstanceTransitionHistoryRepository
        )
        {
            _flowInstanceTransitionHistoryRepository = flowInstanceTransitionHistoryRepository;
        }

        /// <summary>
        /// 获取分页列表信息
        ///</summary>
        public async Task<PagedResultDto<FlowInstanceTransitionHistoryListDto>> GetPaged(GetFlowInstanceTransitionHistorysInput input)
        {
            var query = _flowInstanceTransitionHistoryRepository.GetAll();
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<FlowInstanceTransitionHistoryListDto>>();
            return new PagedResultDto<FlowInstanceTransitionHistoryListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 通过指定id获取
        /// </summary>
        public async Task<FlowInstanceTransitionHistoryListDto> GetById(EntityDto<string> input)
        {
            var entity = await _flowInstanceTransitionHistoryRepository.GetAsync(input.Id);
            return entity.MapTo<FlowInstanceTransitionHistoryListDto>();
        }

        /// <summary>
        /// 获取编辑 
        /// </summary>
        public async Task<GetFlowInstanceTransitionHistoryForEditOutput> GetForEdit(EntityDto<string> input)
        {
            var output = new GetFlowInstanceTransitionHistoryForEditOutput();
            FlowInstanceTransitionHistoryEditDto editDto;
            if (!string.IsNullOrEmpty(input.Id))
            {
                var entity = await _flowInstanceTransitionHistoryRepository.GetAsync(input.Id);
                editDto = entity.MapTo<FlowInstanceTransitionHistoryEditDto>();
            }
            else
            {
                editDto = new FlowInstanceTransitionHistoryEditDto();
            }
            output.FlowInstanceTransitionHistory = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改
        /// </summary>
        public async Task CreateOrUpdate(CreateOrUpdateFlowInstanceTransitionHistoryInput input)
        {
            if (!string.IsNullOrEmpty(input.FlowInstanceTransitionHistory.Id))
            {
                await Update(input.FlowInstanceTransitionHistory);
            }
            else
            {
                await Create(input.FlowInstanceTransitionHistory);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        protected virtual async Task<FlowInstanceTransitionHistoryEditDto> Create(FlowInstanceTransitionHistoryEditDto input)
        {
            var entity = input.MapTo<FlowInstanceTransitionHistory>();
            entity = await _flowInstanceTransitionHistoryRepository.InsertAsync(entity);
            return entity.MapTo<FlowInstanceTransitionHistoryEditDto>();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        protected virtual async Task Update(FlowInstanceTransitionHistoryEditDto input)
        {
            var entity = await _flowInstanceTransitionHistoryRepository.GetAsync(input.Id);
            input.MapTo(entity);
            await _flowInstanceTransitionHistoryRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task Delete(EntityDto<string> input)
        {
            await _flowInstanceTransitionHistoryRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public async Task BatchDelete(List<string> input)
        {
            await _flowInstanceTransitionHistoryRepository.DeleteAsync(s => input.Contains(s.Id));
        }
    }
}
