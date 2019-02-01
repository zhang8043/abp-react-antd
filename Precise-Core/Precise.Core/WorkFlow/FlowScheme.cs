using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Precise.WorkFlow
{
    /// <summary>
    /// 工作流模板信息表
    /// </summary>
    public class FlowScheme : FullAuditedEntity<string>, IPassivable
    {
        /// <summary>
	    /// 流程编号
	    /// </summary>
        public string SchemeCode { get; set; }
        /// <summary>
	    /// 流程名称
	    /// </summary>
        public string SchemeName { get; set; }
        /// <summary>
	    /// 流程分类
	    /// </summary>
        public string SchemeType { get; set; }
        /// <summary>
	    /// 流程内容版本
	    /// </summary>
        public string SchemeVersion { get; set; }
        /// <summary>
	    /// 流程模板使用者
	    /// </summary>
        public string SchemeCanUser { get; set; }
        /// <summary>
	    /// 流程内容
	    /// </summary>
        public string SchemeContent { get; set; }
        /// <summary>
	    /// 表单ID
	    /// </summary>
        public string FrmId { get; set; }
        /// <summary>
	    /// 表单类型
	    /// </summary>
        public int FrmType { get; set; }
        /// <summary>
	    /// 模板权限类型：0完全公开,1指定部门/人员
	    /// </summary>
        public int AuthorizeType { get; set; }
        /// <summary>
	    /// 排序码
	    /// </summary>
        public int SortCode { get; set; }
        /// <summary>
	    /// 有效
	    /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
	    /// 备注
	    /// </summary>
        public string Description { get; set; }
    }
}
