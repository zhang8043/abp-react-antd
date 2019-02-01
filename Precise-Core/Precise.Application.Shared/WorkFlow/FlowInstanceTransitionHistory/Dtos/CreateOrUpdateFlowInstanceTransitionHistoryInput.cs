using System.ComponentModel.DataAnnotations;

namespace Precise.WorkFlow.Dtos
{
    public class CreateOrUpdateFlowInstanceTransitionHistoryInput
    {
        [Required]
        public FlowInstanceTransitionHistoryEditDto FlowInstanceTransitionHistory { get; set; }
    }
}