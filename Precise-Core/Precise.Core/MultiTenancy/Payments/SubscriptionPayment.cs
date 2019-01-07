using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Editions;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace Precise.MultiTenancy.Payments
{
    [Table("AppSubscriptionPayments")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class SubscriptionPayment : FullAuditedEntity<long>
    {
        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public SubscriptionPaymentStatus Status { get; set; }

        public int EditionId { get; set; }

        public int TenantId { get; set; }

        public int DayCount { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public string PaymentId { get; set; }

        public Edition Edition { get; set; }

        public string InvoiceNo { get; set; }

        public void Cancel()
        {
            if (Status == SubscriptionPaymentStatus.Processing)
            {
                Status = SubscriptionPaymentStatus.Cancelled;
            }
        }
    }
}
