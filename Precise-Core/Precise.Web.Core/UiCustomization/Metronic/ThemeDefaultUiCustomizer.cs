using System.Threading.Tasks;
using Abp;
using Abp.Configuration;
using Precise.Configuration;
using Precise.Configuration.Dto;
using Precise.UiCustomization;
using Precise.UiCustomization.Dto;

namespace Precise.Web.UiCustomization.Metronic
{
    public class ThemeDefaultUiCustomizer : UiThemeCustomizerBase, IUiCustomizer
    {
        public ThemeDefaultUiCustomizer(SettingManager settingManager)
            : base(settingManager, AppConsts.ThemeDefault)
        {
        }

        public async Task<UiCustomizationSettingsDto> GetUiSettings()
        {
            var settings = new UiCustomizationSettingsDto
            {
                BaseSettings = new ThemeSettingsDto
                {
                    Layout = new ThemeLayoutSettingsDto
                    {
                        OverallStyle = await GetSettingValueAsync(AppSettings.UiManagement.OverallStyle),
                        ThemeColor = await GetSettingValueAsync(AppSettings.UiManagement.ThemeColor)
                    },
                    Header = new ThemeHeaderSettingsDto
                    {
                        ContentWidth = await GetSettingValueAsync(AppSettings.UiManagement.Header.ContentWidth),
                        FixedHeader = await GetSettingValueAsync<bool>(AppSettings.UiManagement.Header.FixedHeader),
                        SlidingHiddenHeader = await GetSettingValueAsync<bool>(AppSettings.UiManagement.Header.SlidingHiddenHeader),
                    },
                    Menu = new ThemeMenuSettingsDto
                    {
                        NavigationMode = await GetSettingValueAsync(AppSettings.UiManagement.LeftAside.NavigationMode),
                        FixedMenu = await GetSettingValueAsync<bool>(AppSettings.UiManagement.LeftAside.FixedMenu),
                    },
                    Footer = new ThemeFooterSettingsDto
                    {
                        FixedFooter = await GetSettingValueAsync<bool>(AppSettings.UiManagement.Footer.FixedFooter)
                    },
                    Other = new ThemeOtherSettingsDto
                    {
                        WeakMode = await GetSettingValueAsync<bool>(AppSettings.UiManagement.Other.WeakMode)
                    }
                }
            };

            settings.BaseSettings.Theme = ThemeName;

            settings.IsLeftMenuUsed = true;
            settings.IsTopMenuUsed = false;
            settings.IsTabMenuUsed = false;

            return settings;
        }

        public async Task UpdateUserUiManagementSettingsAsync(UserIdentifier user, ThemeSettingsDto settings)
        {
            await SettingManager.ChangeSettingForUserAsync(user, AppSettings.UiManagement.Theme, ThemeName);

            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.ThemeColor, settings.Layout.ThemeColor.ToString());
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.OverallStyle, settings.Layout.OverallStyle.ToString());
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.Header.SlidingHiddenHeader, settings.Header.SlidingHiddenHeader.ToString());
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.Header.ContentWidth, settings.Header.ContentWidth);
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.Header.FixedHeader, settings.Header.FixedHeader.ToString());
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.LeftAside.NavigationMode, settings.Menu.NavigationMode.ToString());
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.LeftAside.FixedMenu, settings.Menu.NavigationMode.ToString());
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.Other.WeakMode, settings.Other.WeakMode.ToString());
            await ChangeSettingForUserAsync(user, AppSettings.UiManagement.Footer.FixedFooter, settings.Footer.FixedFooter.ToString());
        }

        public async Task UpdateTenantUiManagementSettingsAsync(int tenantId, ThemeSettingsDto settings)
        {
            await SettingManager.ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.Theme, settings.Theme);

            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.ThemeColor, settings.Layout.ThemeColor.ToString());
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.OverallStyle, settings.Layout.OverallStyle.ToString());
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.Header.SlidingHiddenHeader, settings.Header.SlidingHiddenHeader.ToString());
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.Header.ContentWidth, settings.Header.ContentWidth);
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.Header.FixedHeader, settings.Header.FixedHeader.ToString());
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.LeftAside.NavigationMode, settings.Menu.NavigationMode.ToString());
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.LeftAside.FixedMenu, settings.Menu.FixedMenu.ToString());
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.Other.WeakMode, settings.Other.WeakMode.ToString());
            await ChangeSettingForTenantAsync(tenantId, AppSettings.UiManagement.Footer.FixedFooter, settings.Footer.FixedFooter.ToString());
        }

        public async Task UpdateApplicationUiManagementSettingsAsync(ThemeSettingsDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.UiManagement.Theme, settings.Theme);

            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.ThemeColor, settings.Layout.ThemeColor.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.OverallStyle, settings.Layout.OverallStyle.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.Header.SlidingHiddenHeader, settings.Header.SlidingHiddenHeader.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.Header.ContentWidth, settings.Header.ContentWidth.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.Header.FixedHeader, settings.Header.FixedHeader.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.LeftAside.NavigationMode, settings.Menu.NavigationMode.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.LeftAside.FixedMenu, settings.Menu.FixedMenu.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.Other.WeakMode, settings.Other.WeakMode.ToString());
            await ChangeSettingForApplicationAsync(AppSettings.UiManagement.Footer.FixedFooter, settings.Footer.FixedFooter.ToString());
        }

        public async Task<ThemeSettingsDto> GetHostUiManagementSettings()
        {
            var theme = await SettingManager.GetSettingValueForApplicationAsync(AppSettings.UiManagement.Theme);

            return new ThemeSettingsDto
            {
                Theme = theme,
                Layout = new ThemeLayoutSettingsDto
                {
                    OverallStyle = await GetSettingValueForApplicationAsync(AppSettings.UiManagement.OverallStyle),
                    ThemeColor = await GetSettingValueForApplicationAsync(AppSettings.UiManagement.ThemeColor)
                },
                Header = new ThemeHeaderSettingsDto
                {
                    SlidingHiddenHeader = await GetSettingValueForApplicationAsync<bool>(AppSettings.UiManagement.Header.SlidingHiddenHeader),
                    ContentWidth = await GetSettingValueForApplicationAsync(AppSettings.UiManagement.Header.ContentWidth),
                    FixedHeader = await GetSettingValueForApplicationAsync<bool>(AppSettings.UiManagement.Header.FixedHeader),
                },
                Menu = new ThemeMenuSettingsDto
                {
                    NavigationMode = await GetSettingValueForApplicationAsync(AppSettings.UiManagement.LeftAside.NavigationMode),
                    FixedMenu = await GetSettingValueForApplicationAsync<bool>(AppSettings.UiManagement.LeftAside.FixedMenu),
                },
                Footer = new ThemeFooterSettingsDto
                {
                    FixedFooter = await GetSettingValueForApplicationAsync<bool>(AppSettings.UiManagement.Footer.FixedFooter)
                },
                Other = new ThemeOtherSettingsDto
                {
                    WeakMode = await GetSettingValueForApplicationAsync<bool>(AppSettings.UiManagement.Other.WeakMode)
                }
            };
        }

        public async Task<ThemeSettingsDto> GetTenantUiCustomizationSettings(int tenantId)
        {
            var theme = await SettingManager.GetSettingValueForTenantAsync(AppSettings.UiManagement.Theme, tenantId);

            return new ThemeSettingsDto
            {
                Theme = theme,
                Layout = new ThemeLayoutSettingsDto
                {
                    OverallStyle = await GetSettingValueForTenantAsync(AppSettings.UiManagement.OverallStyle, tenantId),
                    ThemeColor = await GetSettingValueForTenantAsync(AppSettings.UiManagement.ThemeColor, tenantId)
                },
                Header = new ThemeHeaderSettingsDto
                {
                    SlidingHiddenHeader = await GetSettingValueForTenantAsync<bool>(AppSettings.UiManagement.Header.SlidingHiddenHeader, tenantId),
                    FixedHeader = await GetSettingValueForTenantAsync<bool>(AppSettings.UiManagement.Header.FixedHeader, tenantId),
                    ContentWidth = await GetSettingValueForTenantAsync(AppSettings.UiManagement.Header.ContentWidth, tenantId),
                },
                Menu = new ThemeMenuSettingsDto
                {
                    NavigationMode = await GetSettingValueForTenantAsync(AppSettings.UiManagement.LeftAside.NavigationMode, tenantId),
                    FixedMenu = await GetSettingValueForTenantAsync<bool>(AppSettings.UiManagement.LeftAside.FixedMenu, tenantId),
                },
                Footer = new ThemeFooterSettingsDto
                {
                    FixedFooter = await GetSettingValueForTenantAsync<bool>(AppSettings.UiManagement.Footer.FixedFooter, tenantId)
                },
                Other = new ThemeOtherSettingsDto
                {
                    WeakMode = await GetSettingValueForTenantAsync<bool>(AppSettings.UiManagement.Other.WeakMode, tenantId)
                }
            };
        }
    }
}