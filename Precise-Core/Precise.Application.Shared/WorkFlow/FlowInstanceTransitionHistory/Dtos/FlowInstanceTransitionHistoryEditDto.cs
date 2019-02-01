namespace Precise.WorkFlow.Dtos
{
    public class FlowInstanceTransitionHistoryEditDto
    {

        public string Id { get; set; }         

		public string InstanceId { get; set; }

		public string FromNodeId { get; set; }

		public int? FromNodeType { get; set; }

		public string FromNodeName { get; set; }

		public string ToNodeId { get; set; }

		public int? ToNodeType { get; set; }

		public string ToNodeName { get; set; }

		public int TransitionSate { get; set; }

		public bool IsActive { get; set; }
    }
}