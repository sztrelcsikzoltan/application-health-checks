using InvestmentManager.Core;
using InvestmentManager.DataAccess.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;

var builder = WebApplication.CreateBuilder(args);
// NLog 
NLog.LogManager.LoadConfiguration("nlog.config");

string connectionString = builder.Configuration.GetConnectionString("InvestmentDatabase");

// create an instance of logger factory
using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    // .AddConsole()
    );


// Add services to the container.

// Configure logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.AddNLog();
});

builder.Services.AddControllersWithViews();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// use the logger factory to create an instance of Ilogger
ILogger logger = loggerFactory.CreateLogger<Program>();
builder.Services.RegisterEfDataAccessClasses(connectionString, loggerFactory);

// For Application Services
string stockIndexServiceUrl = builder.Configuration["StockIndexServiceUrl"];
builder.Services.AddStockIndexServiceHttpClientWithoutProfiler(stockIndexServiceUrl);
builder.Services.AddInvestmentManagerServices(stockIndexServiceUrl);

// HEALTH CHECKS

// Liveness healh check
builder.Services.AddHealthChecks();

// Readiness health checks

// Depencency health check of SQL server using AddCheck
builder.Services.AddHealthChecks()
      .AddCheck("SQLServer in startup", () =>
      {
          using (var connection = new SqlConnection(connectionString))
          {
              try
              {
                  connection.Open();
                  using var command = new SqlCommand() { Connection = connection, CommandText = "SELECT 1", CommandTimeout = 1 };
                  // Console.WriteLine("Health check of SQL connection successful.");
                  return HealthCheckResult.Healthy();
              }
              catch (SqlException sqlEx)
              {
                  return HealthCheckResult.Unhealthy(sqlEx.Message);
              }
          }
      });

// SQL server health check using the package AspNetCore.HealthChecks.SqlServer
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "SqlServer", failureStatus: HealthStatus.Unhealthy, tags: new[] { "ready" }, timeout: TimeSpan.FromSeconds(1));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // CONFIGURE HEALTH CHECKS
    
    // Liveness health check
    endpoints.MapHealthChecks("/health");

    // Liveness health check for specific host name(s) and port number(s)
    endpoints.MapHealthChecks("/health-on-host").RequireHost(hosts: new string[] { "localhost:51500", "localhost:51501" } );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
