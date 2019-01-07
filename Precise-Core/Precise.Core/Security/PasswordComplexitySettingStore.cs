using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Zero.Configuration;

namespace Precise.Security
{
    public class PasswordComplexitySettingStore : IPasswordComplexitySettingStore, ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        public PasswordComplexitySettingStore(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task<PasswordComplexitySetting> GetSettingsAsync()
        {
            return new PasswordComplexitySetting
            {
                RequireDigit = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await _settingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await _settingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };
        }
    }
}