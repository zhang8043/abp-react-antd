using System;
using System.Threading.Tasks;
using Abp.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Precise.Configuration;
using Precise.UiCustomization;
using Precise.Web.UiCustomization.Metronic;

namespace Precise.Web.UiCustomization
{
    public class UiThemeCustomizerFactory : IUiThemeCustomizerFactory
    {
        private readonly ISettingManager _settingManager;
        private readonly IServiceProvider _serviceProvider;

        public UiThemeCustomizerFactory(
            ISettingManager settingManager,
            IServiceProvider serviceProvider)
        {
            _settingManager = settingManager;
            _serviceProvider = serviceProvider;
        }

        public async Task<IUiCustomizer> GetCurrentUiCustomizer()
        {
            var theme = await _settingManager.GetSettingValueAsync(AppSettings.UiManagement.Theme);
            return GetUiCustomizerInternal(theme);
        }

        public IUiCustomizer GetUiCustomizer(string theme)
        {
            return GetUiCustomizerInternal(theme);
        }

        private IUiCustomizer GetUiCustomizerInternal(string theme)
        {
            return _serviceProvider.GetService<ThemeDefaultUiCustomizer>();
        }
    }
}