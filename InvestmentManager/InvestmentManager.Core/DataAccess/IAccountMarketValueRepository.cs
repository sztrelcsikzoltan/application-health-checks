using InvestmentManager.Core.Domain;
using System;
using System.Collections.Generic;

namespace InvestmentManager.Core.DataAccess
{
    public interface IAccountMarketValueRepository
    {
        List<AccountMarketValue> LoadAccountMarketValues(String accountNumber);
    }
}
