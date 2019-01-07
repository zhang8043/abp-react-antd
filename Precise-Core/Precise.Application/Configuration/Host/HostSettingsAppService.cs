using System;
using System.Globalization;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero.Configuration;
using Precise.Authorization;
using Precise.Configuration.Host.Dto;
using Precise.Editions;
using Precise.Security;
using Precise.Timing;

namespace Precise.Configuration.Host
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Settings)]
    public class HostSettingsAppService : SettingsAppServiceBase, IHostSettingsAppService
    {
        private readonly EditionManager _editionManager;
        private readonly ITimeZoneService _timeZoneService;
        readonly ISettingDefinitionManager _settingDefinitionManager;

        public HostSettingsAppService(
            IEmailSender emailSender,
            EditionManager editionManager,
            ITimeZoneService timeZoneService,
            ISettingDefinitionManager settingDefinitionManager) : base(emailSender)
        {
            _editionManager = editionManager;
            _timeZoneService = timeZoneService;
            _settingDefinitionManager = settingDefinitionManager;
        }

        #region Get Settings

        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            return new HostSettingsEditDto
            {
                General = await GetGeneralSettingsAsync(),
                TenantManagement = await GetTenantManagementSettingsAsync(),
                UserManagement = await GetUserManagementAsync(),
                Email = await GetEmailSettingsAsync(),
                Security = await GetSecuritySettingsAsync(),
                Billing = await GetBillingSettingsAsync()
            };
        }

        private async Task<GeneralSettingsEditDto> GetGeneralSettingsAsync()
        {
            var timezone = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            var settings = new GeneralSettingsEditDto
            {
                Timezone = timezone,
                TimezoneForComparison = timezone
            };

            var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        private async Task<TenantManagementSettingsEditDto> GetTenantManagementSettingsAsync()
        {
            var settings = new TenantManagementSettingsEditDto
            {
                AllowSelfRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.AllowSelfRegistration),
                IsNewRegisteredTenantActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault),
                UseCaptchaOnRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration)
            };

            var defaultEditionId = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.DefaultEdition);
            if (!string.IsNullOrEmpty(defaultEditionId) && (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionId)) != null))
            {
                settings.DefaultEditionId = Convert.ToInt32(defaultEditionId);
            }

            return settings;
        }

        private async Task<HostUserManagementSettingsEditDto> GetUserManagementAsync()
        {
            return new HostUserManagementSettingsEditDto
            {
                IsEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin),
                SmsVerificationEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SmsVerificationEnabled),
                IsCookieConsentEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsCookieConsentEnabled)
            };
        }

        private async Task<EmailSettingsEditDto> GetEmailSettingsAsync()
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsEditDto
            {
                DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }

        private async Task<SecuritySettingsEditDto> GetSecuritySettingsAsync()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };

            var defaultPasswordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit).DefaultValue),
                RequireLowercase = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase).DefaultValue),
                RequireNonAlphanumeric = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric).DefaultValue),
                RequireUppercase = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase).DefaultValue),
                RequiredLength = Convert.ToInt32(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength).DefaultValue)
            };

            return new SecuritySettingsEditDto
            {
                UseDefaultPasswordComplexitySettings = passwordComplexitySetting.Equals(defaultPasswordComplexitySetting),
                PasswordComplexity = passwordComplexitySetting,
                DefaultPasswordComplexity = defaultPasswordComplexitySetting,
                UserLockOut = await GetUserLockOutSettingsAsync(),
                TwoFactorLogin = await GetTwoFactorLoginSettingsAsync()
            };
        }

        private async Task<HostBillingSettingsEditDto> GetBillingSettingsAsync()
        {
            return new HostBillingSettingsEditDto
            {
                LegalName = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingLegalName),
                Address = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingAddress)
            };
        }

        private async Task<UserLockOutSettingsEditDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds)
            };
        }

        private async Task<TwoFactorLoginSettingsEditDto> GetTwoFactorLoginSettingsAsync()
        {
            var twoFactorLoginSettingsEditDto = new TwoFactorLoginSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                IsEmailProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled),
                IsSmsProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled),
                IsRememberBrowserEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled),
                IsGoogleAuthenticatorEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled)
            };
            return twoFactorLoginSettingsEditDto;
        }

        #endregion

        #region Update Settings

        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await UpdateGeneralSettingsAsync(input.General);
            await UpdateTenantManagementAsync(input.TenantManagement);
            await UpdateUserManagementSettingsAsync(input.UserManagement);
            await UpdateSecuritySettingsAsync(input.Security);
            await UpdateEmailSettingsAsync(input.Email);
            await UpdateBillingSettingsAsync(input.Billing);
        }

        private async Task UpdateBillingSettingsAsync(HostBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingLegalName, input.LegalName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingAddress, input.Address);

        }

        private async Task UpdateGeneralSettingsAsync(GeneralSettingsEditDto settings)
        {
            if (Clock.SupportsMultipleTimezone)
            {
                if (settings.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, settings.Timezone);
                }
            }
        }

        private async Task UpdateTenantManagementAsync(TenantManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.AllowSelfRegistration,
                settings.AllowSelfRegistration.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault,
                settings.IsNewRegisteredTenantActiveByDefault.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.UseCaptchaOnRegistration,
                settings.UseCaptchaOnRegistration.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.DefaultEdition,
                settings.DefaultEditionId?.ToString() ?? ""
            );
        }

        private async Task UpdateUserManagementSettingsAsync(HostUserManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SmsVerificationEnabled,
                settings.SmsVerificationEnabled.ToString().ToLowerInvariant()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.IsCookieConsentEnabled,
                settings.IsCookieConsentEnabled.ToString().ToLowerInvariant()
            );
        }

        private async Task UpdateSecuritySettingsAsync(SecuritySettingsEditDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await UpdatePasswordComplexitySettingsAsync(settings.DefaultPasswordComplexity);
            }
            else
            {
                await UpdatePasswordComplexitySettingsAsync(settings.PasswordComplexity);
            }

            await UpdateUserLockOutSettingsAsync(settings.UserLockOut);
            await UpdateTwoFactorLoginSettingsAsync(settings.TwoFactorLogin);
        }

        private async Task UpdatePasswordComplexitySettingsAsync(PasswordComplexitySetting settings)
        {

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit,
                settings.RequireDigit.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase,
                settings.RequireLowercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric,
                settings.RequireNonAlphanumeric.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase,
                settings.RequireUppercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength,
                settings.RequiredLength.ToString()
            );
        }

        private async Task UpdateUserLockOutSettingsAsync(UserLockOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled, settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds, settings.DefaultAccountLockoutSeconds.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout, settings.MaxFailedAccessAttemptsBeforeLockout.ToString());
        }

        private async Task UpdateTwoFactorLoginSettingsAsync(TwoFactorLoginSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled, settings.IsEmailProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled, settings.IsSmsProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled, settings.IsGoogleAuthenticatorEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, settings.IsRememberBrowserEnabled.ToString().ToLowerInvariant());
        }

        private async Task UpdateEmailSettingsAsync(EmailSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, settings.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, settings.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, settings.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, settings.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, SimpleStringCipher.Instance.Encrypt(settings.SmtpPassword));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, settings.SmtpEnableSsl.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, settings.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        }

        #endregion
    }
}