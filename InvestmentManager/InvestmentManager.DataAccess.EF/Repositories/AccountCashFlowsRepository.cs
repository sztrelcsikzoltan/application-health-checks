﻿using InvestmentManager.Core.DataAccess;
using InvestmentManager.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InvestmentManager.DataAccess.EF.Repositories
{
    public class AccountCashFlowsRepository : IAccountCashFlowsRepository
    {
        public AccountCashFlowsRepository(InvestmentContext context)
        {
            this.dbContext = context;
        }

        protected InvestmentContext dbContext;

        public List<CashFlow> LoadAllAccountCashFlows(string accountNumber)
        {
            return this.dbContext.AccountCashFlows
                .Include(cashFlow => cashFlow.CashFlowType)
                .Where(cashFlow => cashFlow.AccountNumber == accountNumber)
                .ToList();
        }

        public List<CashFlow> LoadExternalCashFlows(string accountNumber)
        {
            return this.dbContext.AccountCashFlows
                .Include(cashFlow => cashFlow.CashFlowType)
                .Where(cashFlow => cashFlow.AccountNumber == accountNumber
                    && (cashFlow.CashFlowType != null ? cashFlow.CashFlowType.IsExternal == true : false))
                .ToList();
        }
    }
}
