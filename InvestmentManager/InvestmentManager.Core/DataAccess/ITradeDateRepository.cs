using InvestmentManager.Core.Domain;
using System.Collections.Generic;

namespace InvestmentManager.Core.DataAccess
{
    public interface ITradeDateRepository
    {
        List<TradeDate> LoadTradeDates();

        TradeDate GetLatestTradeDate();
    }
}
