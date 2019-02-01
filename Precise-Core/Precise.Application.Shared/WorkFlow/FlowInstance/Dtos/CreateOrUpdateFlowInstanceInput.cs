using System.ComponentModel.DataAnnotations;

namespace Precise.WorkFlow.Dtos
{
    public class CreateOrUpdateFlowInstanceInput
    {
        [Required]
        public FlowInstanceEditDto FlowInstance { get; set; }
    }
}