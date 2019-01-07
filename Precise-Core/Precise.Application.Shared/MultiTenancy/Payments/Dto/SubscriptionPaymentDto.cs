namespace Precise.MultiTenancy.Payments.Dto
{
    public class SubscriptionPaymentDto
    {
        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public int EditionId { get; set; }

        public int TenantId { get; set; }

        public int DayCount { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public string PaymentId { get; set; }

        public string PayerId { get; set; }

        public string EditionDisplayName { get; set; }

        public long InvoiceNo { get; set; }
    }
}
