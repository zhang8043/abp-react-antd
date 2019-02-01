namespace Precise.DataItems.Dtos
{
    public class ItemsEntityEditDto
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        public string EnCode { get; set; }

        public string FullName { get; set; }

        public int? SortCode { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}