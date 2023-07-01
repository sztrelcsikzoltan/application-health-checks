using InvestmentManager.Core.DataAccess;
using InvestmentManager.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace InvestmentManager.DataAccess.EF.Repositories
{
    public class TradeDateRepository : ITradeDateRepository
    {

        public TradeDateRepository(InvestmentContext context)
        {
            this.dbContext = context;
        }

        private InvestmentContext dbContext;

        public List<Core.Domain.TradeDate> LoadTradeDates()
        {
            return this.dbContext.TradeDates.ToList();
        }

        public TradeDate GetLatestTradeDate()
        {
            return this.dbContext.TradeDates
                .OrderByDescending(d => d.Date)
                .First();
        }
    }
}
