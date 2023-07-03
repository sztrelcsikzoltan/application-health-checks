using IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentityServer(options =>
// to get more error logs
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiResources(Config.GetApis())
    .AddInMemoryClients(Config.GetClients())
    .AddDeveloperSigningCredential()
    .AddInMemoryPersistedGrants();
    // furhter options if needed
    //.AddInMemoryIdentityResources(GetIdentityResources())
    //.AddInMemoryApiResources(GetApiResources())
    //.AddInMemoryClients(GetClients())
    //.AddAspNetIdentity<User>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

app.UseIdentityServer();

app.Run();
