using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Shared.Configurations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Infrastructure.Extensions;

namespace Basket.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
        services.AddSingleton(eventBusSettings);

        var cacheSettings = configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
        services.AddSingleton(cacheSettings);

        var grpcSettings = configuration.GetSection(nameof(GrpcSettings)).Get<GrpcSettings>();
        services.AddSingleton(grpcSettings);

        var backgroundJobSettings = configuration.GetSection(nameof(BackgroundJobSettings)).Get<BackgroundJobSettings>();
        services.AddSingleton(backgroundJobSettings);

        return services;
    }
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddTransient<ISerializeService, SerializeService>();
        //services.AddTransient<IEmailTemplateService, BasketEmailTemplateService>();
       // services.AddTransient<LoggingDelegatingHandler>();
       // services.ConfigureHealthChecks();
        return services;
    }
    
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.GetOptions<CacheSettings>("CacheSettings");
        if(string.IsNullOrEmpty(settings.ConnectionStrings))
        {
            throw new ArgumentNullException("Redis Connection string is not configured.");
        }
        
        //Redis Configuration
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = settings.ConnectionStrings;
        });
    }

    //public static void ConfigureMassTransit(this IServiceCollection services)
    //{
    //    var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
    //    if (string.IsNullOrEmpty(settings.HostAddress))
    //    {
    //        throw new ArgumentNullException("EventBusSettings is not configured.");
    //    }

    //    var mqConnection = new Uri(settings.HostAddress);
    //    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
    //    services.AddMassTransit(config =>
    //    {
    //        config.UsingRabbitMq((ctx, cfg) =>
    //        {
    //            cfg.Host(mqConnection);
    //        });

    //        config.AddRequestClient<IBasketCheckoutEvent>();
    //    });
    //}
    
    //public static void ConfigureHttpClientService(this IServiceCollection services)
    //{
    //    services.AddHttpClient<BackgroundJobHttpService>()
    //        .AddHttpMessageHandler<LoggingDelegatingHandler>()
    //        .UseImmediateHttpRetryPolicy()
    //        .UseCircuitBreakerPolicy()
    //        .ConfigureTimeoutPolicy();
    //}

    //public static IServiceCollection ConfigureGrpcServices(this IServiceCollection services)
    //{
    //    var settings = services.GetOptions<GrpcSettings>(nameof(GrpcSettings));

    //    services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(x 
    //        => x.Address = new Uri(settings.StockUrl));
    //    services.AddScoped<StockItemGrpcService>();
    //    return services;
    //}
    //private static void ConfigureHealthChecks(this IServiceCollection services)
    //{
    //    var cacheSettings = services.GetOptions<CacheSettings>(nameof(CacheSettings));
    //    services.AddHealthChecks()
    //        .AddRedis(cacheSettings.ConnectionStrings, "Redis Health", HealthStatus.Degraded);
    //}
}