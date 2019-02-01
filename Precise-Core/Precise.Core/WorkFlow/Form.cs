using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Precise.WorkFlow
{
    /// <summary>
    /// 表单模板表
    /// </summary>
    public class Form : FullAuditedEntity<string>, IPassivable
    {
        /// <summary>
	    /// 表单名称
	    /// </summary>
        public string Name { get; set; }
        /// <summary>
	    /// 字段个数
	    /// </summary>
        public int Fields { get; set; }
        /// <summary>
	    /// 表单中的字段数据
	    /// </summary>
        public string ContentData { get; set; }
        /// <summary>
	    /// 表单替换的模板 经过处理
	    /// </summary>
        public string ContentParse { get; set; }
        /// <summary>
	    /// 表单原html模板未经处理的
	    /// </summary>
        public string Content { get; set; }
        /// <summary>
	    /// 排序码
	    /// </summary>
        public int SortCode { get; set; }
        /// <summary>
	    /// 删除标记
	    /// </summary>
        public int Delete { get; set; }
        /// <summary>
	    /// 数据库名称
	    /// </summary>
        public string DbName { get; set; }
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
