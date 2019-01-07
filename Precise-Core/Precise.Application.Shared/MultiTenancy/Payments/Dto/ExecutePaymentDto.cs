using System.Collections.Generic;
using Precise.Editions;

namespace Precise.MultiTenancy.Payments.Dto
{
    public class ExecutePaymentDto
    {
        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public int EditionId { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public Dictionary<string, string> AdditionalData { get; set; }

        public ExecutePaymentDto()
        {
            AdditionalData = new Dictionary<string, string>();
        }
    }
}
