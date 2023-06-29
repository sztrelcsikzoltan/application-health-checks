using StockIndexWebService.Domain;
using System;

namespace StockIndexWebService.ViewModels
{
    public class StockIndexPriceViewModel
    {
        public string? IndexName { get; set; }
        public string? IndexShortDisplayName { get; set; }

        public string? IndexCode { get; set; }
        public DateTime TradeDate { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal AdjustedClosePrice { get; set; }
        public long Volume { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }

        public virtual StockIndex? Index { get; set; }
    }
}
