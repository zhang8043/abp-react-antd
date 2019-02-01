using Abp.Application.Services.Dto;

namespace Precise.WorkFlow.Dtos
{
    public class FlowInstanceOperationHistoryListDto : FullAuditedEntityDto<string> 
    {
		public string InstanceId { get; set; }
		public string Content { get; set; }
    }
}