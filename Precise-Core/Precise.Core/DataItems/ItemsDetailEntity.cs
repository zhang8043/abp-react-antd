using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Precise.DataItems
{
    /// <summary>
    /// 字典项
    /// </summary>
    public class ItemsDetailEntity : FullAuditedEntity<string>, IPassivable
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 项编号
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 项名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool? IsDefault { get; set; }
        /// <summary>
        /// 排序吗
        /// </summary>
        public int? SortCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
