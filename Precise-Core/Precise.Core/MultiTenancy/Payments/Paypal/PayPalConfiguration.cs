using Abp.Dependency;
using Abp.Extensions;
using Microsoft.Extensions.Configuration;
using Precise.Configuration;

namespace Precise.MultiTenancy.Payments.Paypal
{
    public class PayPalConfiguration : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public string Environment => _appConfiguration["Payment:PayPal:Environment"];

        public string BaseUrl => _appConfiguration["Payment:PayPal:BaseUrl"].EnsureEndsWith('/');

        public string ClientId => _appConfiguration["Payment:PayPal:ClientId"];

        public string ClientSecret => _appConfiguration["Payment:PayPal:ClientSecret"];

        public string DemoUsername => _appConfiguration["Payment:PayPal:DemoUsername"];

        public string DemoPassword => _appConfiguration["Payment:PayPal:DemoPassword"];

        public PayPalConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}