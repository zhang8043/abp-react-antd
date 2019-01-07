using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Precise.MultiTenancy.Accounting
{
    [Table("AppInvoices")]
    public class Invoice : Entity<int>
    {
        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string TenantLegalName { get; set; }

        public string TenantAddress { get; set; }

        public string TenantTaxNo { get; set; }
    }
}
