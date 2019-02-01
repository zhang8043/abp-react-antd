using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using Precise.Auditing.Dto;
using Precise.Authorization.Accounts.Dto;
using Precise.Authorization.Permissions.Dto;
using Precise.Authorization.Roles;
using Precise.Authorization.Roles.Dto;
using Precise.Authorization.Users;
using Precise.Authorization.Users.Dto;
using Precise.Authorization.Users.Profile.Dto;
using Precise.Chat;
using Precise.Chat.Dto;
using Precise.DataItems;
using Precise.DataItems.Dtos;
using Precise.Editions;
using Precise.Editions.Dto;
using Precise.Friendships;
using Precise.Friendships.Cache;
using Precise.Friendships.Dto;
using Precise.Localization.Dto;
using Precise.MultiTenancy;
using Precise.MultiTenancy.Dto;
using Precise.MultiTenancy.HostDashboard.Dto;
using Precise.MultiTenancy.Payments;
using Precise.MultiTenancy.Payments.Dto;
using Precise.Notifications.Dto;
using Precise.Organizations.Dto;
using Precise.Sessions.Dto;
using Precise.WorkFlow;
using Precise.WorkFlow.Dtos;

namespace Precise
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */

            //工作流
            configuration.CreateMap<FlowInstance, FlowInstanceListDto>();
            configuration.CreateMap<FlowInstanceListDto, FlowInstance>();
            configuration.CreateMap<FlowInstanceEditDto, FlowInstance>();
            configuration.CreateMap<FlowInstance, FlowInstanceEditDto>();

            configuration.CreateMap<FlowScheme, FlowSchemeListDto>();
            configuration.CreateMap<FlowSchemeListDto, FlowScheme>();
            configuration.CreateMap<FlowSchemeEditDto, FlowScheme>();
            configuration.CreateMap<FlowScheme, FlowSchemeEditDto>();

            configuration.CreateMap<FlowInstanceOperationHistory, FlowInstanceOperationHistoryListDto>();
            configuration.CreateMap<FlowInstanceOperationHistoryListDto, FlowInstanceOperationHistory>();
            configuration.CreateMap<FlowInstanceOperationHistoryEditDto, FlowInstanceOperationHistory>();
            configuration.CreateMap<FlowInstanceOperationHistory, FlowInstanceOperationHistoryEditDto>();

            configuration.CreateMap<FlowInstanceTransitionHistory, FlowInstanceTransitionHistoryListDto>();
            configuration.CreateMap<FlowInstanceTransitionHistoryListDto, FlowInstanceTransitionHistory>();
            configuration.CreateMap<FlowInstanceTransitionHistoryEditDto, FlowInstanceTransitionHistory>();
            configuration.CreateMap<FlowInstanceTransitionHistory, FlowInstanceTransitionHistoryEditDto>();

            configuration.CreateMap<Form, FormListDto>();
            configuration.CreateMap<FormListDto, Form>();
            configuration.CreateMap<FormEditDto, Form>();
            configuration.CreateMap<Form, FormEditDto>();

            configuration.CreateMap<ItemsDetailEntity, ItemsDetailEntityListDto>();
            configuration.CreateMap<ItemsDetailEntityListDto, ItemsDetailEntity>();
            configuration.CreateMap<ItemsDetailEntityEditDto, ItemsDetailEntity>();
            configuration.CreateMap<ItemsDetailEntity, ItemsDetailEntityEditDto>();

            configuration.CreateMap<ItemsEntity, ItemsEntityListDto>();
            configuration.CreateMap<ItemsEntityListDto, ItemsEntity>();
            configuration.CreateMap<ItemsEntityEditDto, ItemsEntity>();
            configuration.CreateMap<ItemsEntity, ItemsEntityEditDto>();
        }
    }
}