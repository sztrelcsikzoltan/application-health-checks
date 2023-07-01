using InvestmentManager.Core.DataAccess;
using InvestmentManager.DataAccess.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace InvestmentManager.DataAccess.EF
{
    public static class IServiceCollectionExtensions
    {
        public static void RegisterEfDataAccessClasses(this IServiceCollection services,
            String connectionString, ILoggerFactory loggerFactory)
        {
            services.AddDbContext<InvestmentContext>(options =>
                options //.UseLazyLoadingProxies()
                .UseSqlServer(connectionString)
                .UseLoggerFactory(loggerFactory)
                );

            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ITradeDateRepository, TradeDateRepository>();
            services.AddScoped<IInvestmentAccountRepository, InvestmentAccountRepository>();
            services.AddScoped<IAccountPositionsRepository, AccountPositionsRepository>();
            services.AddScoped<IAccountMarketValueRepository, AccountMarketValueRepository>();
            services.AddScoped<IAccountCashFlowsRepository, AccountCashFlowsRepository>();
        }

    }
}
