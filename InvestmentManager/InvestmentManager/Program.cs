using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using InvestmentManager.Core;
using InvestmentManager.DataAccess.EF;
using InvestmentManager.Health_Check_Publishers;
using InvestmentManager.HealthChecks;
using InvestmentManager.QueueMessage;
using InvestmentManager.RateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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

var securityLogFilePath = builder.Configuration["securityLogFilePath"];

// HEALTH CHECKS

// Liveness healh check
builder.Services.AddHealthChecks();

// Readiness health checks

// Depencency health check of SQL server using AddCheck
builder.Services.AddHealthChecks()
      .AddCheck("SQLServer in startup", () =>
      {
          using var connection = new SqlConnection(connectionString);
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
      }, tags: new[] { "ready" });

// SQL server health check using the package AspNetCore.HealthChecks.SqlServer
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "SqlServer", failureStatus: HealthStatus.Unhealthy, tags: new[] { "ready" }, timeout: TimeSpan.FromSeconds(1));

// SQL server health checks defined in extension method
builder.Services.AddHealthChecks()
    .AddSqlServerCheckThroughSqlCommand("SqlServerSqlCommand", tags: new[] { "ready" })
    .AddSqlServerCheckThroughDbContext("SqlServerDbContext", tags: new[] { "ready" });

// Endpoint health check using the package AspNetCore.HealthChecks.Uris
builder.Services.AddHealthChecks()
.AddUrlGroup(new Uri($"{stockIndexServiceUrl}/api/StockIndexes"),
    "Stock Index API Health Check", HealthStatus.Degraded, tags: new[] { "ready" }, timeout: new TimeSpan(0, 0, 5));

// File path write health check with a class instance
builder.Services.AddHealthChecks()
    .AddCheck("File Path Health Check class", new FilePathWriteHealthCheck(securityLogFilePath), HealthStatus.Unhealthy, tags: new[] { "ready" });

// File path write health check with an extension method
builder.Services.AddHealthChecks()
    .AddFilePathWrite(securityLogFilePath, HealthStatus.Unhealthy, tags: new[] { "ready" });

// Add HealthCheckPolicy as authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HealthCheckPolicy", policy =>
        policy.RequireClaim("client_policy", "healthChecks"));
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:50337";
        options.RequireHttpsMetadata = false;
        options.Audience = "InvestmentManagerAPI";
    });

// Add Health Checks UI with in-memory storage
builder.Services.AddHealthChecksUI(options =>
{
    options.AddHealthCheckEndpoint(" HC UI endpoint", "https://localhost:51500/healthui");
})
    .AddInMemoryStorage()
    // Add Health Checks UI with db storage - works only if AddInMemoryStorage is not added
    .AddSqlServerStorage(builder.Configuration.GetConnectionString("HealthChecks"));
    
// Add and configure services required for AspNetCoreRateLimit
RateLimit.ConfigureServices(builder.Services, builder.Configuration);
// This is required to set the default value for AspNetCoreRateLimit.IProcessingStrategy
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(5);
    options.Period = TimeSpan.FromSeconds(10);
    options.Predicate = (check) => check.Tags.Contains("ready");
    options.Timeout = TimeSpan.FromSeconds(20);
});

builder.Services.AddSingleton<IHealthCheckPublisher, HealthCheckQueuePublisher>();
builder.Services.AddTransient<IQueueMessage, RabbitMQQueueMessage>();

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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseIpRateLimiting();

app.UseEndpoints(endpoints =>
{
    // CONFIGURE HEALTH CHECKS

    // Liveness health check
    endpoints.MapHealthChecks("/health");

    // Liveness health check for specific host name(s) and port number(s)
    endpoints.MapHealthChecks("/health-on-host").RequireHost(hosts: new string[] { "localhost:51500", "localhost:51501" });

    // Health check with customized status code (for Degraded)
    endpoints.MapHealthChecks("/health-customized-status-code", new HealthCheckOptions()
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });

    // Separate health check endpoint for health checks with "ready" tag, with authorization policy
    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
    {
        Predicate = (check) => check.Tags.Contains("ready") == true,
        ResponseWriter = HealthCheckReadyResponseWriter.WriteHealthCheckReadyResponse,
        AllowCachingResponses = false
    }).RequireAuthorization("HealthCheckPolicy");
    
    // Separate health check endpoint for health checks without "ready" tag
    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
    {
        Predicate = (check) => check.Tags.Contains("ready") == false,
        ResponseWriter = HealthCheckLiveResponseWriter.WriteHealthCheckLiveResponse,
        AllowCachingResponses = false
    })
        // Add CORS policy to the endpoint
        .RequireCors(builder =>
         {
             builder.WithOrigins("http://example.com", "http://exampleTwo.com");
         });

    endpoints.MapControllers();
    // Add endpoint health checks defined in extension method
    endpoints.MapEndpointHealthChecks();

    // Add endpoint for health check UI
    endpoints.MapHealthChecks("/healthui", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Configure HealthCheckUI endpoints
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/healthchecks-ui";
    options.ApiPath = "/health-ui-api";
});


app.Run();
