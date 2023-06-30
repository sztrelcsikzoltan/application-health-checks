using InvestmentManager.Core.Domain;
using System;
using System.Collections.Generic;

namespace InvestmentManager.Core.DataAccess
{
    public interface IAccountPositionsRepository
    {
        List<AccountPosition> LoadAccountPositions(String accountNumber, TradeDate tradeDate);
    }
}
