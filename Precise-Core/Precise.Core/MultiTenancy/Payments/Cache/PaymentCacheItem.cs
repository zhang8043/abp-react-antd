using System;

namespace Precise.MultiTenancy.Payments.Cache
{
    [Serializable]
    public class PaymentCacheItem
    {
        public const string CacheName = "AppPaymentCache";

        public SubscriptionPaymentGatewayType GateWay { get; set; }

        public string PaymentId { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        private PaymentCacheItem()
        {
            
        }

        public PaymentCacheItem(SubscriptionPaymentGatewayType gateWay, PaymentPeriodType paymentPeriodType, string paymentId)
        {
            GateWay = gateWay;
            PaymentId = paymentId;
            PaymentPeriodType = paymentPeriodType;
        }
    }
}
