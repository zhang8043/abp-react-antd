using System;
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
    public class FlowInstanceOperationHistoryAppService : PreciseAppServiceBase, IFlowInstanceOperationHistoryAppService
    {
        private readonly IRepository<FlowInstanceOperationHistory, string> _elowInstanceOperationHistoryRepository;

        public FlowInstanceOperationHistoryAppService(
        IRepository<FlowInstanceOperationHistory, string> elowInstanceOperationHistoryRepository
        )
        {
            _elowInstanceOperationHistoryRepository = elowInstanceOperationHistoryRepository;
        }

        /// <summary>
        /// 获取分页列表信息
        ///</summary>
        public async Task<PagedResultDto<FlowInstanceOperationHistoryListDto>> GetPaged(GetFlowInstanceOperationHistorysInput input)
        {
            var query = _elowInstanceOperationHistoryRepository.GetAll();
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<FlowInstanceOperationHistoryListDto>>();
            return new PagedResultDto<FlowInstanceOperationHistoryListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 通过指定id获取
        /// </summary>
        public async Task<FlowInstanceOperationHistoryListDto> GetById(EntityDto<string> input)
        {
            var entity = await _elowInstanceOperationHistoryRepository.GetAsync(input.Id);
            return entity.MapTo<FlowInstanceOperationHistoryListDto>();
        }

        /// <summary>
        /// 获取编辑
        /// </summary>
        public async Task<GetFlowInstanceOperationHistoryForEditOutput> GetForEdit(EntityDto<string> input)
        {
            var output = new GetFlowInstanceOperationHistoryForEditOutput();
            FlowInstanceOperationHistoryEditDto editDto;
            if (!string.IsNullOrEmpty(input.Id))
            {
                var entity = await _elowInstanceOperationHistoryRepository.GetAsync(input.Id);
                editDto = entity.MapTo<FlowInstanceOperationHistoryEditDto>();
            }
            else
            {
                editDto = new FlowInstanceOperationHistoryEditDto();
            }
            output.FlowInstanceOperationHistory = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改
        /// </summary>
        public async Task CreateOrUpdate(CreateOrUpdateFlowInstanceOperationHistoryInput input)
        {
            if (!string.IsNullOrEmpty(input.FlowInstanceOperationHistory.Id))
            {
                await Update(input.FlowInstanceOperationHistory);
            }
            else
            {
                await Create(input.FlowInstanceOperationHistory);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        protected virtual async Task<FlowInstanceOperationHistoryEditDto> Create(FlowInstanceOperationHistoryEditDto input)
        {
            var entity = input.MapTo<FlowInstanceOperationHistory>();
            entity = await _elowInstanceOperationHistoryRepository.InsertAsync(entity);
            return entity.MapTo<FlowInstanceOperationHistoryEditDto>();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        protected virtual async Task Update(FlowInstanceOperationHistoryEditDto input)
        {
            var entity = await _elowInstanceOperationHistoryRepository.GetAsync(input.Id);
            input.MapTo(entity);
            await _elowInstanceOperationHistoryRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除信息的方法
        /// </summary>
        public async Task Delete(EntityDto<string> input)
        {
            await _elowInstanceOperationHistoryRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public async Task BatchDelete(List<string> input)
        {
            await _elowInstanceOperationHistoryRepository.DeleteAsync(s => input.Contains(s.Id));
        }
    }
}
