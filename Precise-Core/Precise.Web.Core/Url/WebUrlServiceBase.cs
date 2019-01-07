using System.Collections.Generic;
using System.Linq;
using Abp.Extensions;
using Microsoft.Extensions.Configuration;
using Precise.Configuration;

namespace Precise.Web.Url
{
    public abstract class WebUrlServiceBase
    {
        public const string TenancyNamePlaceHolder = "{TENANCY_NAME}";

        public abstract string WebSiteRootAddressFormatKey { get; }

        public abstract string ServerRootAddressFormatKey { get; }

        public string WebSiteRootAddressFormat => _appConfiguration[WebSiteRootAddressFormatKey] ?? "http://localhost:62114/";

        public string ServerRootAddressFormat => _appConfiguration[ServerRootAddressFormatKey] ?? "http://localhost:62114/";

        public bool SupportsTenancyNameInUrl
        {
            get
            {
                var siteRootFormat = WebSiteRootAddressFormat;
                return !siteRootFormat.IsNullOrEmpty() && siteRootFormat.Contains(TenancyNamePlaceHolder);
            }
        }

        readonly IConfigurationRoot _appConfiguration;

        public WebUrlServiceBase(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }

        public string GetSiteRootAddress(string tenancyName = null)
        {
            return ReplaceTenancyNameInUrl(WebSiteRootAddressFormat, tenancyName);
        }

        public string GetServerRootAddress(string tenancyName = null)
        {
            return ReplaceTenancyNameInUrl(ServerRootAddressFormat, tenancyName);
        }

        public List<string> GetRedirectAllowedExternalWebSites()
        {
            var values = _appConfiguration["App:RedirectAllowedExternalWebSites"];
            return values?.Split(',').ToList() ?? new List<string>();
        }

        private string ReplaceTenancyNameInUrl(string siteRootFormat, string tenancyName)
        {
            if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
            {
                return siteRootFormat;
            }

            if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
            {
                siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);
            }

            if (tenancyName.IsNullOrEmpty())
            {
                return siteRootFormat.Replace(TenancyNamePlaceHolder, "");
            }

            return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + ".");
        }
    }
}