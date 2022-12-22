using AutoMapper;
using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Customer.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<CustomerContext>(options => options.UseNpgsql(connectionString));
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        //services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped(typeof(IRepositoryQueryBase<,,>), typeof(RepositoryQueryBase<,,>));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();

        //services.ConfigureHealthChecks(connectionString);
        return services;
    }

    //private static void ConfigureHealthChecks(this IServiceCollection services, string connectionString)
    //{
    //    services.AddHealthChecks()
    //        .AddNpgSql(connectionString, name: "PostgresQL Health", failureStatus: HealthStatus.Degraded);
    //}
}

