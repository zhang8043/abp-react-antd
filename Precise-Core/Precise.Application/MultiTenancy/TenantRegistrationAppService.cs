using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using Precise.Configuration;
using Precise.Debugging;
using Precise.Editions;
using Precise.Editions.Dto;
using Precise.Features;
using Precise.MultiTenancy.Dto;
using Precise.MultiTenancy.Payments;
using Precise.MultiTenancy.Payments.Cache;
using Precise.Notifications;
using Precise.Security.Recaptcha;
using Precise.Url;
using Abp.Extensions;
using Precise.MultiTenancy.Payments.Dto;

namespace Precise.MultiTenancy
{
    public class TenantRegistrationAppService : PreciseAppServiceBase, ITenantRegistrationAppService
    {
        public IAppUrlService AppUrlService { get; set; }

        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IRecaptchaValidator _recaptchaValidator;
        private readonly EditionManager _editionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly ILocalizationContext _localizationContext;
        private readonly TenantManager _tenantManager;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IPaymentGatewayManagerFactory _paymentGatewayManagerFactory;
        private readonly IPaymentCache _paymentCache;

        public TenantRegistrationAppService(
            IMultiTenancyConfig multiTenancyConfig,
            IRecaptchaValidator recaptchaValidator,
            EditionManager editionManager,
            IAppNotifier appNotifier,
            ILocalizationContext localizationContext,
            TenantManager tenantManager,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IPaymentGatewayManagerFactory paymentGatewayManagerFactory,
            IPaymentCache paymentCache)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _recaptchaValidator = recaptchaValidator;
            _editionManager = editionManager;
            _appNotifier = appNotifier;
            _localizationContext = localizationContext;
            _tenantManager = tenantManager;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _paymentGatewayManagerFactory = paymentGatewayManagerFactory;
            _paymentCache = paymentCache;

            AppUrlService = NullAppUrlService.Instance;
        }

        public async Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input)
        {
            if (input.EditionId.HasValue)
            {
                await CheckEditionSubscriptionAsync(input.EditionId.Value, input.SubscriptionStartType, input.Gateway, input.PaymentId);
            }
            else
            {
                await CheckRegistrationWithoutEdition();
            }

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                CheckTenantRegistrationIsEnabled();

                if (UseCaptchaOnRegistration())
                {
                    await _recaptchaValidator.ValidateAsync(input.CaptchaResponse);
                }

                //Getting host-specific settings
                var isNewRegisteredTenantActiveByDefault = await SettingManager.GetSettingValueForApplicationAsync<bool>(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault);
                var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

                DateTime? subscriptionEndDate = null;
                var isInTrialPeriod = false;

                if (input.EditionId.HasValue)
                {
                    isInTrialPeriod = input.SubscriptionStartType == SubscriptionStartType.Trial;

                    if (isInTrialPeriod)
                    {
                        var edition = (SubscribableEdition)await _editionManager.GetByIdAsync(input.EditionId.Value);
                        subscriptionEndDate = Clock.Now.AddDays(edition.TrialDayCount ?? 0);
                    }
                }

                var tenantId = await _tenantManager.CreateWithAdminUserAsync(
                    input.TenancyName,
                    input.Name,
                    input.AdminPassword,
                    input.AdminEmailAddress,
                    null,
                    isNewRegisteredTenantActiveByDefault,
                    input.EditionId,
                    false,
                    true,
                    subscriptionEndDate,
                    isInTrialPeriod,
                    AppUrlService.CreateEmailActivationUrlFormat(input.TenancyName)
                );

                Tenant tenant;

                if (input.SubscriptionStartType == SubscriptionStartType.Paid)
                {
                    if (!input.Gateway.HasValue)
                    {
                        throw new Exception("Gateway is missing!");
                    }

                    var payment = await _subscriptionPaymentRepository.GetByGatewayAndPaymentIdAsync(
                        input.Gateway.Value,
                        input.PaymentId
                    );

                    tenant = await _tenantManager.UpdateTenantAsync(
                        tenantId,
                        true,
                        false,
                        payment.PaymentPeriodType,
                        payment.EditionId,
                        EditionPaymentType.NewRegistration);

                    await _subscriptionPaymentRepository.UpdateByGatewayAndPaymentIdAsync(input.Gateway.Value,
                        input.PaymentId, tenantId, SubscriptionPaymentStatus.Completed);
                }
                else
                {
                    tenant = await TenantManager.GetByIdAsync(tenantId);
                }

                await _appNotifier.NewTenantRegisteredAsync(tenant);

                if (input.EditionId.HasValue && input.Gateway.HasValue && !input.PaymentId.IsNullOrEmpty())
                {
                    _paymentCache.RemoveCacheItem(input.Gateway.Value, input.PaymentId);
                }

                return new RegisterTenantOutput
                {
                    TenantId = tenant.Id,
                    TenancyName = input.TenancyName,
                    Name = input.Name,
                    UserName = AbpUserBase.AdminUserName,
                    EmailAddress = input.AdminEmailAddress,
                    IsActive = tenant.IsActive,
                    IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin,
                    IsTenantActive = tenant.IsActive
                };
            }
        }

        private async Task CheckRegistrationWithoutEdition()
        {
            var editions = await _editionManager.GetAllAsync();
            if (editions.Any())
            {
                throw new Exception("Tenant registration is not allowed without edition because there are editions defined !");
            }
        }

        public async Task<EditionsSelectOutput> GetEditionsForSelect()
        {
            var features = FeatureManager
                .GetAll()
                .Where(feature => (feature[FeatureMetadata.CustomFeatureKey] as FeatureMetadata)?.IsVisibleOnPricingTable ?? false);

            var flatFeatures = ObjectMapper
                .Map<List<FlatFeatureSelectDto>>(features)
                .OrderBy(f => f.DisplayName)
                .ToList();

            var editions = (await _editionManager.GetAllAsync())
                .Cast<SubscribableEdition>()
                .OrderBy(e => e.MonthlyPrice)
                .ToList();

            var featureDictionary = features.ToDictionary(feature => feature.Name, f => f);

            var editionWithFeatures = new List<EditionWithFeaturesDto>();
            foreach (var edition in editions)
            {
                editionWithFeatures.Add(await CreateEditionWithFeaturesDto(edition, featureDictionary));
            }

            int? tenantEditionId = null;
            if (AbpSession.UserId.HasValue)
            {
                tenantEditionId = (await _tenantManager.GetByIdAsync(AbpSession.GetTenantId()))
                    .EditionId;
            }

            return new EditionsSelectOutput
            {
                AllFeatures = flatFeatures,
                EditionsWithFeatures = editionWithFeatures,
                TenantEditionId = tenantEditionId
            };
        }

        public async Task<EditionSelectDto> GetEdition(int editionId)
        {
            var edition = await _editionManager.GetByIdAsync(editionId);
            var editionDto = ObjectMapper.Map<EditionSelectDto>(edition);
            foreach (var paymentGateway in Enum.GetValues(typeof(SubscriptionPaymentGatewayType)).Cast<SubscriptionPaymentGatewayType>())
            {
                using (var paymentGatewayManager = _paymentGatewayManagerFactory.Create(paymentGateway))
                {
                    var additionalData = await paymentGatewayManager.Object.GetAdditionalPaymentData(ObjectMapper.Map<SubscribableEdition>(edition));
                    editionDto.AdditionalData.Add(paymentGateway, additionalData);
                }
            }

            return editionDto;
        }

        private async Task<EditionWithFeaturesDto> CreateEditionWithFeaturesDto(SubscribableEdition edition, Dictionary<string, Feature> featureDictionary)
        {
            return new EditionWithFeaturesDto
            {
                Edition = ObjectMapper.Map<EditionSelectDto>(edition),
                FeatureValues = (await _editionManager.GetFeatureValuesAsync(edition.Id))
                    .Where(featureValue => featureDictionary.ContainsKey(featureValue.Name))
                    .Select(fv => new NameValueDto(
                        fv.Name,
                        featureDictionary[fv.Name].GetValueText(fv.Value, _localizationContext))
                    )
                    .ToList()
            };
        }

        private void CheckTenantRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfTenantRegistrationIsDisabledMessage_Detail"));
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                throw new UserFriendlyException(L("MultiTenancyIsNotEnabled"));
            }
        }

        private bool IsSelfRegistrationEnabled()
        {
            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.AllowSelfRegistration);
        }

        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration);
        }

        private async Task CheckEditionSubscriptionAsync(int editionId, SubscriptionStartType subscriptionStartType, SubscriptionPaymentGatewayType? gateway, string paymentId)
        {
            var edition = await _editionManager.GetByIdAsync(editionId) as SubscribableEdition;
            
            CheckSubscriptionStart(edition, subscriptionStartType);
            CheckPaymentCache(edition, subscriptionStartType, gateway, paymentId);
        }

        private void CheckPaymentCache(SubscribableEdition edition, SubscriptionStartType subscriptionStartType, SubscriptionPaymentGatewayType? gateway, string paymentId)
        {
            if (edition.IsFree || subscriptionStartType != SubscriptionStartType.Paid)
            {
                return;
            }

            if (!gateway.HasValue)
            {
                throw new Exception("Gateway cannot be empty !");
            }

            if (paymentId.IsNullOrEmpty())
            {
                throw new Exception("PaymentId cannot be empty !");
            }

            var paymentCacheItem = _paymentCache.GetCacheItemOrNull(gateway.Value, paymentId);
            if (paymentCacheItem == null)
            {
                throw new UserFriendlyException(L("PaymentMightBeExpiredWarning"));
            }
        }

        private static void CheckSubscriptionStart(SubscribableEdition edition, SubscriptionStartType subscriptionStartType)
        {
            switch (subscriptionStartType)
            {
                case SubscriptionStartType.Free:
                    if (!edition.IsFree)
                    {
                        throw new Exception("This is not a free edition !");
                    }
                    break;
                case SubscriptionStartType.Trial:
                    if (!edition.HasTrial())
                    {
                        throw new Exception("Trial is not available for this edition !");
                    }
                    break;
                case SubscriptionStartType.Paid:
                    if (edition.IsFree)
                    {
                        throw new Exception("This is a free edition and cannot be subscribed as paid !");
                    }
                    break;
            }
        }
    }
}
