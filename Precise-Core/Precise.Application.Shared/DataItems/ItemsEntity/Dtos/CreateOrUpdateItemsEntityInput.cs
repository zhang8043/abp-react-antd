using System.ComponentModel.DataAnnotations;

namespace Precise.DataItems.Dtos
{
    public class CreateOrUpdateItemsEntityInput
    {
        [Required]
        public ItemsEntityEditDto ItemsEntity { get; set; }

    }
}