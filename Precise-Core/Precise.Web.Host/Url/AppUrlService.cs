using Abp.MultiTenancy;
using Precise.Url;

namespace Precise.Web.Url
{
    public class AppUrlService : AppUrlServiceBase
    {
        public override string EmailActivationRoute => "account/confirm-email";

        public override string PasswordResetRoute => "account/reset-password";

        public AppUrlService(
                IWebUrlService webUrlService,
                ITenantCache tenantCache
            ) : base(
                webUrlService,
                tenantCache
            )
        {

        }
    }
}