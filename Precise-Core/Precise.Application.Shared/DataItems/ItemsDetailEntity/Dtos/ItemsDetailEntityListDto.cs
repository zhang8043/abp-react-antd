using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace Precise.DataItems.Dtos
{
    public class ItemsDetailEntityListDto : FullAuditedEntityDto<string>, IPassivable
    {
        public string ItemId { get; set; }

        public string ParentId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public bool? IsDefault { get; set; }

        public int? SortCode { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}