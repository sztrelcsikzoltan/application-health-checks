using InvestmentManager.Core;
using InvestmentManager.DataAccess.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

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
