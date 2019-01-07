using System.ComponentModel.DataAnnotations;

namespace Precise.Editions.Dto
{
    public class EditionEditDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public int? TrialDayCount { get; set; }

        public int? WaitingDayAfterExpire { get; set; }

        public int? ExpiringEditionId { get; set; }
    }
}