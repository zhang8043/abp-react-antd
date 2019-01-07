using Newtonsoft.Json;

namespace Precise.MultiTenancy.Payments.Paypal
{
    public class Payer
    {
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }
    }
}