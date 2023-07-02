using System;

namespace InvestmentManager.ApiModels
{
    /// <summary>
    /// Represents the position in a security in an account (basically a stock the account holds)
    /// </summary>
    public class AccountPositionModel
    {
        public DateTime Date { get; set; }

        public string? AccountNumber { get; set; }

        public string? Symbol { get; set; }

        public string? SecurityName { get; set; }

        public string? Description { get; set; }

        public string? Sector { get; set; }

        public string? Industry { get; set; }

        public decimal Shares { get; set; }

        public decimal Price { get; set; }

        public decimal MarketValue { get; set; }
    }
}
