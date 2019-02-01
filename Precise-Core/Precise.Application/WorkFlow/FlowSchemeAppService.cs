using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Precise.WorkFlow.Dtos;
using Abp.Authorization;

namespace Precise.WorkFlow
{
    [AbpAuthorize]
    public class FlowSchemeApp : PreciseAppServiceBase, IFlowSchemeAppService
    {
        private readonly IRepository<FlowScheme, string> _flowSchemeRepository;

        public FlowSchemeApp(
        IRepository<FlowScheme, string> flowSchemeRepository
            )
        {
            _flowSchemeRepository = flowSchemeRepository;
        }

        /// <summary>
        /// 获取分页列表信息
        ///</summary>
        public async Task<PagedResultDto<FlowSchemeListDto>> GetPaged(GetFlowSchemesInput input)
        {
            var query = _flowSchemeRepository.GetAll();
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<FlowSchemeListDto>>();
            return new PagedResultDto<FlowSchemeListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 通过指定id获取
        /// </summary>
        public async Task<FlowSchemeListDto> GetById(EntityDto<string> input)
        {
            var entity = await _flowSchemeRepository.GetAsync(input.Id);
            return entity.MapTo<FlowSchemeListDto>();
        }

        /// <summary>
        /// 获取编辑
        /// </summary>
        public async Task<GetFlowSchemeForEditOutput> GetForEdit(EntityDto<string> input)
        {
            var output = new GetFlowSchemeForEditOutput();
            FlowSchemeEditDto editDto;
            if (!string.IsNullOrEmpty(input.Id))
            {
                var entity = await _flowSchemeRepository.GetAsync(input.Id);
                editDto = entity.MapTo<FlowSchemeEditDto>();
            }
            else
            {
                editDto = new FlowSchemeEditDto();
            }
            output.FlowScheme = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改
        /// </summary>
        public async Task CreateOrUpdate(CreateOrUpdateFlowSchemeInput input)
        {
            if (!string.IsNullOrEmpty(input.FlowScheme.Id))
            {
                await Update(input.FlowScheme);
            }
            else
            {
                await Create(input.FlowScheme);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        protected virtual async Task<FlowSchemeEditDto> Create(FlowSchemeEditDto input)
        {
            var entity = input.MapTo<FlowScheme>();
            entity = await _flowSchemeRepository.InsertAsync(entity);
            return entity.MapTo<FlowSchemeEditDto>();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        protected virtual async Task Update(FlowSchemeEditDto input)
        {
            var entity = await _flowSchemeRepository.GetAsync(input.Id);
            input.MapTo(entity);
            await _flowSchemeRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task Delete(EntityDto<string> input)
        {
            await _flowSchemeRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public async Task BatchDelete(List<string> input)
        {
            await _flowSchemeRepository.DeleteAsync(s => input.Contains(s.Id));
        }
    }
}
