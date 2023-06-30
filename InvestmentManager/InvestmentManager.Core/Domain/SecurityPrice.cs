using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentManager.Core.Domain
{
    public class SecurityPrice
    {
        public DateTime Date { get; set; }

        [ForeignKey("Date")]
        // [NotMapped]
        public virtual TradeDate? TradeDate { get; set; }

        public String? Symbol { get; set; }

        [ForeignKey("Symbol")]
        // [NotMapped]
        public virtual Security? Security { get; set; }

        public decimal? OpenPrice { get; set; }

        public decimal? ClosePrice { get; set; }

        public decimal? DailyHigh { get; set; }

        public decimal DailyLow { get; set; }

        public long Volume { get; set; }

        public decimal? Change { get; set; }

        public decimal? ChangePercent { get; set; }
    }
}
