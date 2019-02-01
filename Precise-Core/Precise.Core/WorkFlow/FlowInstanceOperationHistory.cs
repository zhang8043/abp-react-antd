using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Precise.WorkFlow
{
    /// <summary>
    /// 工作流实例操作记录
    /// </summary>
    public class FlowInstanceOperationHistory : FullAuditedEntity<string>
    {
        /// <summary>
	    /// 实例进程Id
	    /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
	    /// 操作内容
	    /// </summary>
        public string Content { get; set; }
    }
}
