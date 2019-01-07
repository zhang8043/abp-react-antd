using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.UI;
using Abp.Zero.EntityFrameworkCore;
using Castle.Core.Internal;
using Precise.Authorization;
using Precise.Authorization.Users;
using Precise.Configuration;
using Precise.Configuration.Host.Dto;
using Precise.EntityFrameworkCore;
using Precise.Identity;
using Precise.Install.Dto;
using Precise.Migrations.Seed;
using Precise.Migrations.Seed.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Precise.Install
{
    [AbpAllowAnonymous]
    [DisableAuditing]
    public class InstallAppService : PreciseAppServiceBase, IInstallAppService
    {
        private readonly AbpZeroDbMigrator<PreciseDbContext> _migrator;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly DatabaseCheckHelper _databaseCheckHelper;

        public InstallAppService(AbpZeroDbMigrator migrator,
            LogInManager logInManager,
            SignInManager signInManager, 
            DatabaseCheckHelper databaseCheckHelper)
        {
            _migrator = migrator;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _databaseCheckHelper = databaseCheckHelper;
        }

        public async Task Setup(InstallDto input)
        {
            if (CheckDatabaseInternal())
            {
                throw new UserFriendlyException("Setup process is already done.");
            }

            SetConnectionString(input.ConnectionString);

            _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);

            if (CheckDatabaseInternal())
            {
                await SetAdminPassword(input.AdminPassword);
                SetUrl(input.WebSiteUrl, input.ServerUrl);
                await SetDefaultLanguage(input.DefaultLanguage);
                await SetSmtpSettings(input.SmtpSettings);
                await SetBillingSettings(input.BillInfo);
            }
            else
            {
                throw new UserFriendlyException("Database couldn't be created!");
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public AppSettingsJsonDto GetAppSettingsJson()
        {
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var appUrl = (JObject)appsettingsjson["App"];

            if (appUrl.Property("WebSiteRootAddress").IsNullOrEmpty())
                return new AppSettingsJsonDto
                {
                    WebSiteUrl = appUrl.Property("ClientRootAddress").Value.ToString(),
                    ServerSiteUrl = appUrl.Property("ServerRootAddress").Value.ToString(),
                    Languages = DefaultLanguagesCreator.InitialLanguages.Select(l => new NameValue(l.DisplayName, l.Name)).ToList()
                };

            return new AppSettingsJsonDto
            {
                WebSiteUrl = appUrl.Property("WebSiteRootAddress").Value.ToString()
            };
        }

        public CheckDatabaseOutput CheckDatabase()
        {
            return new CheckDatabaseOutput
            {
                IsDatabaseExist = CheckDatabaseInternal()
            };
        }

        private bool CheckDatabaseInternal()
        {
            var connectionString = GetConnectionString();

            if (string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            return _databaseCheckHelper.Exist(connectionString);
        }

        private string GetConnectionString()
        {
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));
            var connectionStrings = (JObject)appsettingsjson["ConnectionStrings"];
            return connectionStrings.Property(PreciseConsts.ConnectionStringName).Value.ToString();
        }

        private void SetConnectionString(string constring)
        {
            EditAppSettingsjson("Default", constring, "ConnectionStrings");
        }

        private async Task SetAdminPassword(string adminPassword)
        {
            var admin = await UserManager.FindByIdAsync("1");

            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var loginResult = await _logInManager.LoginAsync(User.AdminUserName, "123qwe");
            var signInResult = await _signInManager.SignInOrTwoFactorAsync(loginResult, false);
            if (signInResult.Succeeded)
            {
                CheckErrors(await UserManager.ChangePasswordAsync(admin, adminPassword));
                admin.ShouldChangePasswordOnNextLogin = false;
                CheckErrors(await UserManager.UpdateAsync(admin));
            }
        }

        private void SetUrl(string webSitRUrl, string serverUrl)
        {
            if (!serverUrl.IsNullOrEmpty())
            {
                EditAppSettingsjson("ClientRootAddress", webSitRUrl, "App");
                EditAppSettingsjson("ServerRootAddress", serverUrl, "App");
            }
            else
            {
                EditAppSettingsjson("WebSiteRootAddress", webSitRUrl, "App");
            }
        }

        private async Task SetDefaultLanguage(string language)
        {
            await SettingManager.ChangeSettingForApplicationAsync(LocalizationSettingNames.DefaultLanguage, language);
        }

        private async Task SetSmtpSettings(EmailSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, input.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, input.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, input.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, input.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, SimpleStringCipher.Instance.Encrypt(input.SmtpPassword));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, input.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, input.SmtpEnableSsl.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, input.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        }

        private async Task SetBillingSettings(HostBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingLegalName, input.LegalName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingAddress, input.Address);
        }

        private void EditAppSettingsjson(string name, string value, params string[] objectNames)
        {
            var appsettingsjson = JObject.Parse(File.ReadAllText("appsettings.json"));

            var jobj = appsettingsjson;

            foreach (var objectName in objectNames)
            {
                jobj = (JObject)jobj[objectName];
            }

            jobj.Property(name).Value.Replace(value);

            using (var file = File.CreateText("appsettings.json"))
            using (var writer = new JsonTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;
                appsettingsjson.WriteTo(writer);
            }
        }
    }
}
