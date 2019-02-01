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
    public class ItemsDetailEntityAppService : PreciseAppServiceBase, IItemsDetailEntityAppService
    {
        private readonly IRepository<ItemsDetailEntity, string> _itemsDetailEntityRepository;

        public ItemsDetailEntityAppService(
        IRepository<ItemsDetailEntity, string> itemsDetailEntityRepository
        )
        {
            _itemsDetailEntityRepository = itemsDetailEntityRepository;

        }
        public async Task<PagedResultDto<ItemsDetailEntityListDto>> GetPaged(GetItemsDetailEntitysInput input)
        {
            var query = _itemsDetailEntityRepository.GetAll();
            var count = await query.CountAsync();
            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();
            var entityListDtos = entityList.MapTo<List<ItemsDetailEntityListDto>>();
            return new PagedResultDto<ItemsDetailEntityListDto>(count, entityListDtos);
        }
        public async Task<ItemsDetailEntityListDto> GetById(EntityDto<string> input)
        {
            var entity = await _itemsDetailEntityRepository.GetAsync(input.Id);
            return entity.MapTo<ItemsDetailEntityListDto>();
        }
        public async Task<GetItemsDetailEntityForEditOutput> GetForEdit(EntityDto<string> input)
        {
            var output = new GetItemsDetailEntityForEditOutput();
            ItemsDetailEntityEditDto editDto;
            if (!string.IsNullOrEmpty(input.Id))
            {
                var entity = await _itemsDetailEntityRepository.GetAsync(input.Id);
                editDto = entity.MapTo<ItemsDetailEntityEditDto>();
            }
            else
            {
                editDto = new ItemsDetailEntityEditDto();
            }
            output.ItemsDetailEntity = editDto;
            return output;
        }
        public async Task CreateOrUpdate(CreateOrUpdateItemsDetailEntityInput input)
        {
            if (!string.IsNullOrEmpty(input.ItemsDetailEntity.Id))
            {
                await Update(input.ItemsDetailEntity);
            }
            else
            {
                await Create(input.ItemsDetailEntity);
            }
        }
        protected virtual async Task<ItemsDetailEntityEditDto> Create(ItemsDetailEntityEditDto input)
        {
            var entity = input.MapTo<ItemsDetailEntity>();
            entity = await _itemsDetailEntityRepository.InsertAsync(entity);
            return entity.MapTo<ItemsDetailEntityEditDto>();
        }
        protected virtual async Task Update(ItemsDetailEntityEditDto input)
        {
            var entity = await _itemsDetailEntityRepository.GetAsync(input.Id);
            input.MapTo(entity);
            await _itemsDetailEntityRepository.UpdateAsync(entity);
        }
        public async Task Delete(EntityDto<string> input)
        {
            await _itemsDetailEntityRepository.DeleteAsync(input.Id);
        }
        public async Task BatchDelete(List<string> input)
        {
            await _itemsDetailEntityRepository.DeleteAsync(s => input.Contains(s.Id));
        }
    }
}
