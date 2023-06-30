using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentManager.Core.Domain
{
    public class AccountMarketValue
    {
        public DateTime Date { get; set; }
        
        [ForeignKey("Date")]
        public virtual TradeDate? TradeDate { get; set; }

        public String? AccountNumber { get; set; }

        public decimal MarketValue { get; set; }

    }
}
