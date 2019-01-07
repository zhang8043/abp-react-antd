using Newtonsoft.Json;

namespace Precise.MultiTenancy.Payments.Paypal
{
    public class PayPalAccessTokenResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}