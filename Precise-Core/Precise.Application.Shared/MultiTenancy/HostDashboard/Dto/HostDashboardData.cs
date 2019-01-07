using System;
using System.Collections.Generic;

namespace Precise.MultiTenancy.HostDashboard.Dto
{
    public class HostDashboardData
    {
        public int NewTenantsCount { get; set; }
        public decimal NewSubscriptionAmount { get; set; }
        public int DashboardPlaceholder1 { get; set; }
        public int DashboardPlaceholder2 { get; set; }
        public List<IncomeStastistic> IncomeStatistics { get; set; }
        public List<TenantEdition> EditionStatistics { get; set; }
        public List<ExpiringTenant> ExpiringTenants { get; set; }
        public List<RecentTenant> RecentTenants { get; set; }
        public int MaxExpiringTenantsShownCount { get; set; }
        public int MaxRecentTenantsShownCount { get; set; }
        public int SubscriptionEndAlertDayCount { get; set; }
        public int RecentTenantsDayCount { get; set; }
        public DateTime SubscriptionEndDateStart { get; set; }
        public DateTime SubscriptionEndDateEnd { get; set; }
        public DateTime TenantCreationStartDate { get; set; }
    }
}

