using InvestmentManager.Core.DataAccess;
using InvestmentManager.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestmentManager.DataAccess.EF.Repositories
{
    public class InvestmentAccountRepository : IInvestmentAccountRepository
    {
        public InvestmentAccountRepository(InvestmentContext context)
        {
            this.dbContext = context;
        }

        protected InvestmentContext dbContext;

        public InvestmentAccount LoadInvestmentAccount(String accountNumber, TradeDate tradeDate)
        {
            return this.dbContext.Accounts
                .Include(account => account.AccountType)
                .Where(account => account.AccountNumber == accountNumber)
                .Select(acct => new
                {
                    Account = acct,
                    MarketValue = this.dbContext.AccountMarketValues
                        .Where(mv => mv.AccountNumber == acct.AccountNumber && mv.Date == tradeDate.Date)
                        .Sum(mv => mv.MarketValue)
                }
                ).AsEnumerable()
                .Select(x =>
                {
                    x.Account.MarketValue = x.MarketValue;
                    return x.Account;
                }).First();
        }

        public IEnumerable<InvestmentAccount> LoadInvestmentAccounts(TradeDate tradeDate)
        {
            return this.dbContext.Accounts
                .Include(account => account.AccountType)
                .Select(acct => new
                {
                    Account = acct,
                    MarketValue = this.dbContext.AccountMarketValues
                        .Where(mv => mv.AccountNumber == acct.AccountNumber && mv.Date == tradeDate.Date)
                        .Sum(mv => mv.MarketValue)
                }
                ).AsEnumerable()
                .Select(x =>
                {
                    x.Account.MarketValue = x.MarketValue;
                    return x.Account;
                }).ToList();
        }
    }



}
