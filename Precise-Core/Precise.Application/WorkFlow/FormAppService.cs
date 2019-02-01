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
    public class FormAppService : PreciseAppServiceBase, IFormAppService
    {
        private readonly IRepository<Form, string> _formRepository;

        public FormAppService(
        IRepository<Form, string> formRepository
        )
        {
            _formRepository = formRepository;
        }

        /// <summary>
        /// 获取Form的分页列表信息
        ///</summary>
        public async Task<PagedResultDto<FormListDto>> GetPaged(GetFormsInput input)
        {
            var query = _formRepository.GetAll();
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<FormListDto>>();
            return new PagedResultDto<FormListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 通过指定id获取FormListDto信息
        /// </summary>
        public async Task<FormListDto> GetById(EntityDto<string> input)
        {
            var entity = await _formRepository.GetAsync(input.Id);
            return entity.MapTo<FormListDto>();
        }

        /// <summary>
        /// 获取编辑 Form
        /// </summary>
        public async Task<GetFormForEditOutput> GetForEdit(EntityDto<string> input)
        {
            var output = new GetFormForEditOutput();
            FormEditDto editDto;
            if (!string.IsNullOrEmpty(input.Id))
            {
                var entity = await _formRepository.GetAsync(input.Id);
                editDto = entity.MapTo<FormEditDto>();
            }
            else
            {
                editDto = new FormEditDto();
            }
            output.Form = editDto;
            return output;
        }

        /// <summary>
        /// 添加或者修改Form的公共方法
        /// </summary>
        public async Task CreateOrUpdate(CreateOrUpdateFormInput input)
        {
            if (!string.IsNullOrEmpty(input.Form.Id))
            {
                await Update(input.Form);
            }
            else
            {
                await Create(input.Form);
            }
        }

        /// <summary>
        /// 新增Form
        /// </summary>
        protected virtual async Task<FormEditDto> Create(FormEditDto input)
        {
            var entity = input.MapTo<Form>();
            entity = await _formRepository.InsertAsync(entity);
            return entity.MapTo<FormEditDto>();
        }

        /// <summary>
        /// 编辑Form
        /// </summary>
        protected virtual async Task Update(FormEditDto input)
        {
            var entity = await _formRepository.GetAsync(input.Id);
            input.MapTo(entity);
            await _formRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除Form信息的方法
        /// </summary>
        public async Task Delete(EntityDto<string> input)
        {
            await _formRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Form的方法
        /// </summary>
        public async Task BatchDelete(List<string> input)
        {
            await _formRepository.DeleteAsync(s => input.Contains(s.Id));
        }
    }
}
