using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Timing;
using Microsoft.EntityFrameworkCore;
using Precise.Authorization;
using Precise.MultiTenancy.HostDashboard.Dto;
using Precise.MultiTenancy.Payments;

namespace Precise.MultiTenancy.HostDashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class HostDashboardAppService : PreciseAppServiceBase, IHostDashboardAppService
    {
        private const int SubscriptionEndAlertDayCount = 30;
        private const int MaxExpiringTenantsShownCount = 10;
        private const int MaxRecentTenantsShownCount = 10;
        private const int RecentTenantsDayCount = 7;

        private readonly IRepository<SubscriptionPayment, long> _subscriptionPaymentRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IIncomeStatisticsService _incomeStatisticsService;

        public HostDashboardAppService(IRepository<SubscriptionPayment, long> subscriptionPaymentRepository,
            IRepository<Tenant> tenantRepository,
            IIncomeStatisticsService incomeStatisticsService)
        {
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _tenantRepository = tenantRepository;
            _incomeStatisticsService = incomeStatisticsService;
        }

        public async Task<HostDashboardData> GetDashboardStatisticsData(GetDashboardDataInput input)
        {
            var subscriptionEndDateEndUtc = Clock.Now.ToUniversalTime().AddDays(SubscriptionEndAlertDayCount);
            var subscriptionEndDateStartUtc = Clock.Now.ToUniversalTime();
            var tenantCreationStartDate = Clock.Now.ToUniversalTime().AddDays(-RecentTenantsDayCount);

            return new HostDashboardData
            {
                DashboardPlaceholder1 = 125,
                DashboardPlaceholder2 = 830,
                NewTenantsCount = await GetTenantsCountByDate(input.StartDate, input.EndDate),
                NewSubscriptionAmount = await GetNewSubscriptionAmount(input.StartDate, input.EndDate),
                IncomeStatistics = await _incomeStatisticsService.GetIncomeStatisticsData(input.StartDate, input.EndDate, input.IncomeStatisticsDateInterval),
                EditionStatistics = await GetEditionTenantStatisticsData(input.StartDate, input.EndDate),
                ExpiringTenants = await GetExpiringTenantsData(subscriptionEndDateStartUtc, subscriptionEndDateEndUtc, MaxExpiringTenantsShownCount),
                RecentTenants = await GetRecentTenantsData(tenantCreationStartDate, MaxRecentTenantsShownCount),
                MaxExpiringTenantsShownCount = MaxExpiringTenantsShownCount,
                MaxRecentTenantsShownCount = MaxRecentTenantsShownCount,
                SubscriptionEndAlertDayCount = SubscriptionEndAlertDayCount,
                RecentTenantsDayCount = RecentTenantsDayCount,
                SubscriptionEndDateStart = subscriptionEndDateStartUtc,
                SubscriptionEndDateEnd = subscriptionEndDateEndUtc,
                TenantCreationStartDate = tenantCreationStartDate
            };
        }

        public async Task<GetIncomeStatisticsDataOutput> GetIncomeStatistics(GetIncomeStatisticsDataInput input)
        {
            return new GetIncomeStatisticsDataOutput(await _incomeStatisticsService.GetIncomeStatisticsData(input.StartDate, input.EndDate, input.IncomeStatisticsDateInterval));
        }
 
        public async Task<GetEditionTenantStatisticsOutput> GetEditionTenantStatistics(GetEditionTenantStatisticsInput input)
        {
            return new GetEditionTenantStatisticsOutput(await GetEditionTenantStatisticsData(input.StartDate, input.EndDate));
        }

        private async Task<List<TenantEdition>> GetEditionTenantStatisticsData(DateTime startDate, DateTime endDate)
        {
            return await _tenantRepository.GetAll()
                .Where(t => t.EditionId.HasValue &&
                            t.IsActive &&
                            t.CreationTime >= startDate &&
                            t.CreationTime <= endDate)
                .GroupBy(t => t.Edition)
                .Select(t => new TenantEdition
                {
                    Label = t.Key.DisplayName,
                    Value = t.Count()
                })
                .OrderBy(t => t.Label)
                .ToListAsync();
        }

        private async Task<decimal> GetNewSubscriptionAmount(DateTime startDate, DateTime endDate)
        {
            return await _subscriptionPaymentRepository.GetAll()
                .Where(s => s.CreationTime >= startDate &&
                            s.CreationTime <= endDate &&
                            s.Status == SubscriptionPaymentStatus.Completed)
                .Select(x => x.Amount)
                .DefaultIfEmpty(0)
                .SumAsync();
        }

        private async Task<int> GetTenantsCountByDate(DateTime startDate, DateTime endDate)
        {
            return await _tenantRepository.GetAll()
                .Where(t => t.CreationTime >= startDate && t.CreationTime <= endDate)
                .CountAsync();
        }

        private async Task<List<ExpiringTenant>> GetExpiringTenantsData(DateTime subscriptionEndDateStartUtc, DateTime subscriptionEndDateEndUtc, int? maxExpiringTenantsShownCount = null)
        {
            var query = _tenantRepository.GetAll().Where(t =>
                    t.SubscriptionEndDateUtc.HasValue &&
                    t.SubscriptionEndDateUtc.Value >= subscriptionEndDateStartUtc &&
                    t.SubscriptionEndDateUtc.Value <= subscriptionEndDateEndUtc)
                .Select(t => new ExpiringTenant
                {
                    TenantName = t.Name,
                    RemainingDayCount = Convert.ToInt32(t.SubscriptionEndDateUtc.Value.Subtract(subscriptionEndDateStartUtc).TotalDays)
                });

            if (maxExpiringTenantsShownCount.HasValue)
            {
                query = query.Take(maxExpiringTenantsShownCount.Value);
            }

            return await query.OrderBy(t => t.RemainingDayCount).ThenBy(t => t.TenantName).ToListAsync();
        }

        private async Task<List<RecentTenant>> GetRecentTenantsData(DateTime creationDateStart, int? maxRecentTenantsShownCount = null)
        {
            var query = _tenantRepository.GetAll()
                .Where(t => t.CreationTime >= creationDateStart)
                .OrderByDescending(t => t.CreationTime);

            if (maxRecentTenantsShownCount.HasValue)
            {
                query = (IOrderedQueryable<Tenant>)query.Take(maxRecentTenantsShownCount.Value);
            }

            return await query.Select(t => ObjectMapper.Map<RecentTenant>(t)).ToListAsync();
        }
    }
}