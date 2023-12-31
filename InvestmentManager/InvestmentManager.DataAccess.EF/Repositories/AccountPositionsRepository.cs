﻿using InvestmentManager.Core.DataAccess;
using InvestmentManager.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InvestmentManager.DataAccess.EF.Repositories
{
    public class AccountPositionsRepository : IAccountPositionsRepository
    {
        public AccountPositionsRepository(InvestmentContext context)
        {
            this.dbContext = context;
        }

        protected InvestmentContext dbContext;

        public List<AccountPosition> LoadAccountPositions(string? accountNumber, TradeDate tradeDate)
        {
            return this.dbContext.AccountPositions
                .Include(p => p.Security)
                .Where(p => p.AccountNumber == accountNumber && (p.Date == tradeDate.Date))
                .ToList();
        }
    }
}
