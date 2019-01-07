using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Precise.MultiTenancy.HostDashboard.Dto;

namespace Precise.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}