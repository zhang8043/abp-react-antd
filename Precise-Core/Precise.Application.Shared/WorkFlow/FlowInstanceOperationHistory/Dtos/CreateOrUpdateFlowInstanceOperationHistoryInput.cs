using System.ComponentModel.DataAnnotations;

namespace Precise.WorkFlow.Dtos
{
    public class CreateOrUpdateFlowInstanceOperationHistoryInput
    {
        [Required]
        public FlowInstanceOperationHistoryEditDto FlowInstanceOperationHistory { get; set; }
    }
}