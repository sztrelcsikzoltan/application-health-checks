using System;

namespace InvestmentManager.Core.Domain
{
    /// <summary>
    /// Represents a date that US Markets were open and trading occured
    /// </summary>
    public class TradeDate
    {
        /// <summary>
        /// Gets the date of this TradeDate
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Checks if this Trade Date was the last trading day of the month
        /// </summary>
        public bool IsMonthEnd { get; set; }

        /// <summary>
        /// Checks if this Trade Date was the last trading day of the quarter
        /// </summary>
        public bool IsQuarterEnd { get; set; }

        /// <summary>
        /// Checks if this Trade Date was the last trading day of the calendar year
        /// </summary>
        public bool IsYearEnd { get; set; }

        /// <summary>
        /// Checks if two TradeDate objects represent the same date
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            // if (obj != null && obj is DateTime)
            if (obj is not null and DateTime)
            {
                DateTime other = (DateTime)obj;
                return other.Date.Date.Equals(this.Date.Date);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Date.Date.GetHashCode();
        }
    }
}
