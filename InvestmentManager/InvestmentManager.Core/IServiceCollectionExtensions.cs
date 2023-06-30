using InvestmentManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InvestmentManager.Core
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInvestmentManagerServices(this IServiceCollection services, 
            String stockIndexUrl)
        {
            services.AddScoped<RateOfReturnService, RateOfReturnService>();
            services.AddSingleton<StockIndexService, StockIndexService>();
        }

        public static void AddStockIndexServiceHttpClientWithoutProfiler(
            this IServiceCollection services, String serviceUrl)
        {
            services.AddHttpClient("StockIndexApi", c =>
            {
                c.BaseAddress = new Uri(serviceUrl);
            });
        }

    }
}
