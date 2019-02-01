using System.ComponentModel.DataAnnotations;

namespace Precise.WorkFlow.Dtos
{
    public class CreateOrUpdateFormInput
    {
        [Required]
        public FormEditDto Form { get; set; }
    }
}