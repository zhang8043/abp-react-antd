namespace Precise.MultiTenancy.Payments.Paypal
{
    public class PayPalPaymentGatewayPaymentStatusConverter : IPaymentGatewayPaymentStatusConverter
    {
        public SubscriptionPaymentStatus ConvertToSubscriptionPaymentStatus(string externalStatus)
        {
            if (externalStatus == "approved")
            {
                return SubscriptionPaymentStatus.Completed;
            }

            if (externalStatus == "created")
            {
                return SubscriptionPaymentStatus.Processing;
            }

            return SubscriptionPaymentStatus.Failed;
        }
    }
}