using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Precise.WorkFlow
{
    /// <summary>
    /// 工作流实例流转历史记录
    /// </summary>
    public class FlowInstanceTransitionHistory : FullAuditedEntity<string>, IPassivable
    {
        /// <summary>
	    /// 实例Id
	    /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
	    /// 开始节点Id
	    /// </summary>
        public string FromNodeId { get; set; }
        /// <summary>
	    /// 开始节点类型
	    /// </summary>
        public int? FromNodeType { get; set; }
        /// <summary>
	    /// 开始节点名称
	    /// </summary>
        public string FromNodeName { get; set; }
        /// <summary>
	    /// 结束节点Id
	    /// </summary>
        public string ToNodeId { get; set; }
        /// <summary>
	    /// 结束节点类型
	    /// </summary>
        public int? ToNodeType { get; set; }
        /// <summary>
	    /// 结束节点名称
	    /// </summary>
        public string ToNodeName { get; set; }
        /// <summary>
	    /// 转化状态
	    /// </summary>
        public int TransitionSate { get; set; }
        /// <summary>
	    /// 是否结束
	    /// </summary>
        public bool IsActive { get; set; }
    }
}
