using System.ComponentModel.DataAnnotations;

namespace Precise.DataItems.Dtos
{
    public class CreateOrUpdateItemsDetailEntityInput
    {
        [Required]
        public ItemsDetailEntityEditDto ItemsDetailEntity { get; set; }
    }
}