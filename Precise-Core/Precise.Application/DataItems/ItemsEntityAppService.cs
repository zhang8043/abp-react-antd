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
using Precise.DataItems.Dtos;

namespace Precise.DataItems
{
    [AbpAuthorize]
    public class ItemsEntityAppService : PreciseAppServiceBase, IItemsEntityAppService
    {
        private readonly IRepository<ItemsEntity, string> _itemsEntityRepository;

        public ItemsEntityAppService(
        IRepository<ItemsEntity, string> itemsEntityRepository
        )
        {
            _itemsEntityRepository = itemsEntityRepository;
        }
        public async Task<PagedResultDto<ItemsEntityListDto>> GetPaged(GetItemsEntitysInput input)
        {
            var query = _itemsEntityRepository.GetAll();
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<ItemsEntityListDto>>();
            return new PagedResultDto<ItemsEntityListDto>(count, entityListDtos);
        }
        public async Task<ItemsEntityListDto> GetById(EntityDto<string> input)
        {
            var entity = await _itemsEntityRepository.GetAsync(input.Id);
            return entity.MapTo<ItemsEntityListDto>();
        }
        public async Task<GetItemsEntityForEditOutput> GetForEdit(EntityDto<string> input)
        {
            var output = new GetItemsEntityForEditOutput();
            ItemsEntityEditDto editDto;
            if (!string.IsNullOrEmpty(input.Id))
            {
                var entity = await _itemsEntityRepository.GetAsync(input.Id);
                editDto = entity.MapTo<ItemsEntityEditDto>();
            }
            else
            {
                editDto = new ItemsEntityEditDto();
            }
            output.ItemsEntity = editDto;
            return output;
        }
        public async Task CreateOrUpdate(CreateOrUpdateItemsEntityInput input)
        {
            if (!string.IsNullOrEmpty(input.ItemsEntity.Id))
            {
                await Update(input.ItemsEntity);
            }
            else
            {
                await Create(input.ItemsEntity);
            }
        }
        protected virtual async Task<ItemsEntityEditDto> Create(ItemsEntityEditDto input)
        {
            var entity = input.MapTo<ItemsEntity>();
            entity = await _itemsEntityRepository.InsertAsync(entity);
            return entity.MapTo<ItemsEntityEditDto>();
        }
        protected virtual async Task Update(ItemsEntityEditDto input)
        {
            var entity = await _itemsEntityRepository.GetAsync(input.Id);
            input.MapTo(entity);
            await _itemsEntityRepository.UpdateAsync(entity);
        }
        public async Task Delete(EntityDto<string> input)
        {
            await _itemsEntityRepository.DeleteAsync(input.Id);
        }
        public async Task BatchDelete(List<string> input)
        {
            await _itemsEntityRepository.DeleteAsync(s => input.Contains(s.Id));
        }
    }
}
