using Newtonsoft.Json;

namespace Precise.MultiTenancy.Payments.Paypal
{
    public class PayPalTransaction
    {
        [JsonProperty("amount")]
        public PayPalAmount Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}