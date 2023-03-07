using Contracts.Common.Interfaces;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderContext>(options =>
        {
            options.UseSqlServer("Server=orderdb,1433;Database=OrderDb;User ID=sa;Password=Passw0rd1;",
                   builder =>
                   {
                       builder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName);
                       builder.EnableRetryOnFailure();
                   });
        });

        services.AddScoped<OrderContextSeed>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
       // services.AddScoped(typeof(ISmtpEmailService), typeof(SmtpEmailService));

        return services;
    }
}