using HealthCheckAlerter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<Worker>();
                 
var app = builder.Build();

app.Run();
