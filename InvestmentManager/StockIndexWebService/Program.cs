using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockIndexWebService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

builder.Services.AddDbContext<StockIndexDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("StockIndexDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMvc();

app.Run();
