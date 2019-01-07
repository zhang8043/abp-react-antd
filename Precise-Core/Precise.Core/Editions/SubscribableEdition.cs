using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Editions;
using Precise.MultiTenancy.Payments;

namespace Precise.Editions
{
    /// <summary>
    /// Extends <see cref="Edition"/> to add subscription features.
    /// </summary>
    public class SubscribableEdition : Edition
    {
        /// <summary>
        /// The edition that will assigned after expire date
        /// </summary>
        public int? ExpiringEditionId { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public int? TrialDayCount { get; set; }

        /// <summary>
        /// The account will be taken an action (termination of tenant account) after the specified days when the subscription is expired.
        /// </summary>
        public int? WaitingDayAfterExpire { get; set; }

        [NotMapped]
        public bool IsFree => !MonthlyPrice.HasValue && !AnnualPrice.HasValue;

        public bool HasTrial()
        {
            if (IsFree)
            {
                return false;
            }

            return TrialDayCount.HasValue && TrialDayCount.Value > 0;
        }

        public decimal GetPaymentAmount(PaymentPeriodType? paymentPeriodType)
        {
            if (MonthlyPrice == null || AnnualPrice == null)
            {
                throw new Exception("No price information found for " + DisplayName + " edition!");
            }

            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Monthly:
                    return MonthlyPrice.Value;
                case PaymentPeriodType.Annual:
                    return AnnualPrice.Value;
                default:
                    throw new Exception("Edition does not support payment type: " + paymentPeriodType);
            }
        }

        public bool HasSamePrice(SubscribableEdition edition)
        {
            return !IsFree &&
                   MonthlyPrice == edition.MonthlyPrice && AnnualPrice == edition.AnnualPrice;
        }
    }
}