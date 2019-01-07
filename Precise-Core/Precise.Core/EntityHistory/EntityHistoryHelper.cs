using System;
using System.Linq;
using Abp.Organizations;
using Precise.Authorization.Roles;
using Precise.MultiTenancy;

namespace Precise.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type=>type.FullName)
                .Select(types=>types.First())
                .ToArray();
    }
}
