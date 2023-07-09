using InvestmentManager.Core.DataAccess;
using InvestmentManager.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InvestmentManager.DataAccess.EF.Repositories
{
    public class AccountMarketValueRepository : IAccountMarketValueRepository
    {
        public AccountMarketValueRepository(InvestmentContext context)
        {
            this.dbContext = context;
        }

        protected InvestmentContext dbContext;


        public List<AccountMarketValue> LoadAccountMarketValues(string accountNumber)
        {
            return this.dbContext.AccountMarketValues
                .Include(mv => mv.TradeDate)
                .Where(mv => mv.AccountNumber == accountNumber)
                .ToList();
        }
    }
}
