namespace Precise.Configuration
{
    /// <summary>
    /// Defines string constants for setting names in the application.
    /// See <see cref="AppSettingProvider"/> for setting definitions.
    /// </summary>
    public static class AppSettings
    {
        public static class HostManagement
        {
            public const string BillingLegalName = "App.HostManagement.BillingLegalName";
            public const string BillingAddress = "App.HostManagement.BillingAddress";
        }

        public static class UiManagement
        {
            public const string OverallStyle = "App.UiManagement.OverallStyle";
            public const string Theme = "App.UiManagement.Theme";
            public const string ThemeColor = "App.UiManagement.ThemeColor";

            public static class Header
            {
                public const string SlidingHiddenHeader = "App.UiManagement.Header.SlidingHiddenHeader";
                public const string ContentWidth = "App.UiManagement.Header.ContentWidth"; 
                public const string FixedHeader = "App.UiManagement.Header.FixedHeader"; 
            }

            public static class LeftAside
            {
                public const string NavigationMode = "App.UiManagement.Left.NavigationMode";
                public const string FixedMenu = "App.UiManagement.Left.FixedMenu";
            }

            public static class Footer
            {
                public const string FixedFooter = "App.UiManagement.Footer.FixedFooter";
            }

            public static class Other
            {
                public const string WeakMode = "App.UiManagement.Other.WeakMode";
            }
        }

        public static class TenantManagement
        {
            public const string AllowSelfRegistration = "App.TenantManagement.AllowSelfRegistration";
            public const string IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
            public const string DefaultEdition = "App.TenantManagement.DefaultEdition";
            public const string SubscriptionExpireNotifyDayCount = "App.TenantManagement.SubscriptionExpireNotifyDayCount";
            public const string BillingLegalName = "App.UserManagement.BillingLegalName";
            public const string BillingAddress = "App.UserManagement.BillingAddress";
            public const string BillingTaxVatNo = "App.UserManagement.BillingTaxVatNo";
        }

        public static class UserManagement
        {
            public static class TwoFactorLogin
            {
                public const string IsGoogleAuthenticatorEnabled = "App.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled";
            }

            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
            public const string SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";
            public const string IsCookieConsentEnabled = "App.UserManagement.IsCookieConsentEnabled";
        }

        public static class Recaptcha
        {
            public const string SiteKey = "Recaptcha.SiteKey";
        }

        public static class CacheKeys
        {
            public const string TenantRegistrationCache = "TenantRegistrationCache";
        }
    }
}