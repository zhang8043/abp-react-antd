namespace Precise.DataItems.Dtos
{
    public class ItemsDetailEntityEditDto
    {
        public string Id { get; set; }

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