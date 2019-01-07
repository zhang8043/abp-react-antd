using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Zero.Configuration;
using Microsoft.Extensions.Configuration;

namespace Precise.Configuration
{
    /// <summary>
    /// Defines settings for the application.
    /// See <see cref="AppSettings"/> for setting names.
    /// </summary>
    public class AppSettingProvider : SettingProvider
    {
        private readonly IConfigurationRoot _appConfiguration;

        public AppSettingProvider(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            //Disable TwoFactorLogin by default (can be enabled by UI)
            context.Manager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled).DefaultValue = false.ToString().ToLowerInvariant();

            return GetHostSettings().Union(GetTenantSettings()).Union(GetSharedSettings())
                //theme settings
                .Union(GetDefaultThemeSettings());
        }

        private IEnumerable<SettingDefinition> GetHostSettings()
        {
            return new[] {
                new SettingDefinition(AppSettings.TenantManagement.AllowSelfRegistration, GetFromAppSettings(AppSettings.TenantManagement.AllowSelfRegistration, "true"), isVisibleToClients: true),
                new SettingDefinition(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault, GetFromAppSettings(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault, "false")),
                new SettingDefinition(AppSettings.TenantManagement.UseCaptchaOnRegistration, GetFromAppSettings(AppSettings.TenantManagement.UseCaptchaOnRegistration, "true"), isVisibleToClients: true),
                new SettingDefinition(AppSettings.TenantManagement.DefaultEdition, GetFromAppSettings(AppSettings.TenantManagement.DefaultEdition, "")),
                new SettingDefinition(AppSettings.UserManagement.SmsVerificationEnabled, GetFromAppSettings(AppSettings.UserManagement.SmsVerificationEnabled, "false"), isVisibleToClients: true),
                new SettingDefinition(AppSettings.TenantManagement.SubscriptionExpireNotifyDayCount, GetFromAppSettings(AppSettings.TenantManagement.SubscriptionExpireNotifyDayCount, "7"), isVisibleToClients: true),
                new SettingDefinition(AppSettings.HostManagement.BillingLegalName, GetFromAppSettings(AppSettings.HostManagement.BillingLegalName, "")),
                new SettingDefinition(AppSettings.HostManagement.BillingAddress, GetFromAppSettings(AppSettings.HostManagement.BillingAddress, "")),
                new SettingDefinition(AppSettings.Recaptcha.SiteKey, GetFromSettings("Recaptcha:SiteKey"), isVisibleToClients: true),
                new SettingDefinition(AppSettings.UiManagement.Theme, GetFromAppSettings(AppSettings.UiManagement.Theme, "default"), isVisibleToClients: true, scopes: SettingScopes.All)
            };
        }

        private IEnumerable<SettingDefinition> GetTenantSettings()
        {
            return new[]
            {
                new SettingDefinition(AppSettings.UserManagement.AllowSelfRegistration, GetFromAppSettings(AppSettings.UserManagement.AllowSelfRegistration, "true"), scopes: SettingScopes.Tenant, isVisibleToClients: true),
                new SettingDefinition(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault, GetFromAppSettings(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault, "false"), scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettings.UserManagement.UseCaptchaOnRegistration, GetFromAppSettings(AppSettings.UserManagement.UseCaptchaOnRegistration, "true"), scopes: SettingScopes.Tenant, isVisibleToClients: true),
                new SettingDefinition(AppSettings.TenantManagement.BillingLegalName, GetFromAppSettings(AppSettings.TenantManagement.BillingLegalName, ""), scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettings.TenantManagement.BillingAddress, GetFromAppSettings(AppSettings.TenantManagement.BillingAddress, ""), scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettings.TenantManagement.BillingTaxVatNo, GetFromAppSettings(AppSettings.TenantManagement.BillingTaxVatNo, ""), scopes: SettingScopes.Tenant)
            };
        }

        private IEnumerable<SettingDefinition> GetSharedSettings()
        {
            return new[]
            {
                new SettingDefinition(AppSettings.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled, GetFromAppSettings(AppSettings.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled, "false"), scopes: SettingScopes.Application | SettingScopes.Tenant, isVisibleToClients: true),
                new SettingDefinition(AppSettings.UserManagement.IsCookieConsentEnabled, GetFromAppSettings(AppSettings.UserManagement.IsCookieConsentEnabled, "false"), scopes: SettingScopes.Application | SettingScopes.Tenant, isVisibleToClients: true)
            };
        }

        private string GetFromAppSettings(string name, string defaultValue = null)
        {
            return GetFromSettings("App:" + name, defaultValue);
        }

        private string GetFromSettings(string name, string defaultValue = null)
        {
            return _appConfiguration[name] ?? defaultValue;
        }

        private IEnumerable<SettingDefinition> GetDefaultThemeSettings()
        {
            var themeName = "default";
            //{
            //  "navTheme": "dark",
            //  "primaryColor": "#1890FF",
            //  "layout": "sidemenu",
            //  "contentWidth": "Fluid",
            //  "fixedHeader": true,
            //  "autoHideHeader": true,
            //  "fixSiderbar": true,
            //  "collapse": true
            //}
            return new[]
            {
                //整体风格设置
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.OverallStyle, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.OverallStyle, "dark"), isVisibleToClients: true, scopes: SettingScopes.All),
                //下滑时隐藏 Header
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.Header.SlidingHiddenHeader, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.Header.SlidingHiddenHeader, "true"),isVisibleToClients: true, scopes: SettingScopes.All),
                //内容区域宽度
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.Header.ContentWidth, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.Header.ContentWidth, "Fluid"),isVisibleToClients: true, scopes: SettingScopes.All),
                //固定 Header
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.Header.FixedHeader, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.Header.FixedHeader, "true"),isVisibleToClients: true, scopes: SettingScopes.All),
                //导航模式
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.LeftAside.NavigationMode, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.LeftAside.NavigationMode, "sidemenu"), isVisibleToClients: true, scopes: SettingScopes.All),
                //固定侧边菜单
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.LeftAside.FixedMenu, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.LeftAside.FixedMenu, "true"),isVisibleToClients: true, scopes: SettingScopes.All),
                //色弱模式
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.Other.WeakMode, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.Other.WeakMode, "false"),isVisibleToClients: true, scopes: SettingScopes.All),
                //固定底部
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.Footer.FixedFooter, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.Footer.FixedFooter, "false"),isVisibleToClients: true, scopes: SettingScopes.All),
                //主题色
                new SettingDefinition(themeName + "." + AppSettings.UiManagement.ThemeColor, GetFromAppSettings(themeName + "." +AppSettings.UiManagement.ThemeColor, "#1890FF"), isVisibleToClients: true, scopes: SettingScopes.All)
            };
        }
    }
}
