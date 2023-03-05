using Common.Logging;
using Contracts.Common.Interfaces;
using HealthChecks.UI.Client;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;
using System;
using System.Diagnostics;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} Api up");

// Add services to the container.

try
{
    builder.Host.AddAppConfigurations();
    
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.AddControllers();
    builder.Services.AddScoped<ISerializeService, SerializeService>();
    builder.Services.AddSingleton<Stopwatch>(new Stopwatch());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplicationServices();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.ConfigureHealthChecks(builder.Configuration);
    builder.Services.ConfigureMassTransit();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    using(var scope = app.Services.CreateScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderContextSeed>();
        await orderContextSeed.InitializeAsync();
        await orderContextSeed.TrySeedAsync();
    }
    app.UseRouting();
    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        endpoints.MapDefaultControllerRoute();
    });

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if(type.Equals("StopTheHostException", StringComparison.Ordinal))
        throw;
    Log.Fatal(ex, "Unhanded exception");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} API complete");
    Log.CloseAndFlush();
}