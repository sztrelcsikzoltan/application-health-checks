using InvestmentManager.Core.Domain;

namespace InvestmentManager.Models
{
    public class InvestmentAccountDetailsModel
    {
        public TradeDate? TradeDate { get; set; }

        public InvestmentAccount? InvestmentAccount { get; set; }

        public PeriodSummary? CurrentPerformance { get; set; }
    }
}
