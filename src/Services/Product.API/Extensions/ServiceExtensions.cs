using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        //public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        //    services.AddSingleton(jwtSettings);

        //    var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        //    services.AddSingleton(databaseSettings);

        //    var apiConfigSettings = configuration.GetSection(nameof(ApiConfigurationSettings)).Get<ApiConfigurationSettings>();
        //    Log.Information($"ApiConfigurationSettings data: {JsonSerializer.Serialize(apiConfigSettings)}");
        //    services.AddSingleton(apiConfigSettings);

        //    return services;
        //}

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //services.ConfigureSwagger();

            services.ConfigureProductDbContext(configuration);
             services.AddInfrastructureService();
            //services.AddAutoMapper(config => config.AddProfile(new MappingProfile()));
            //services.ConfigureAuthenticationHandler();
            // services.ConfigureAuthorization();
            //services.ConfigureHealthChecks();

            return services;
        }

        private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString");
            var builder = new MySqlConnectionStringBuilder(connectionString);

            services.AddDbContext<ProductContext>(m => m.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString), e =>
                {
                    e.MigrationsAssembly("Product.API");
                    e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                }));

            return services;
        }

        private static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBase<,,>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
           // services.AddScoped<IProductRepository, ProductRepository>();

            return services;

        }

        //private static void ConfigureHealthChecks(this IServiceCollection services)
        //{
        //    var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
        //    Log.Information($"ConnectionString ConfigureHealthChecks: {JsonSerializer.Serialize(databaseSettings)}");
        //    services.AddHealthChecks()
        //        .AddMySql(databaseSettings.ConnectionString, "MySql Health", HealthStatus.Degraded);
        //}

        //public static void ConfigureSwagger(this IServiceCollection services)
        //{
        //    var configuration = services.GetOptions<ApiConfigurationSettings>(nameof(ApiConfigurationSettings));
        //    if (configuration == null || string.IsNullOrEmpty(configuration.IssuerUri) || string.IsNullOrEmpty(configuration.ApiName))
        //        throw new Exception("ApiConfigurationSettings is not configured!");
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo
        //        {
        //            Title = "Product API v1",
        //            Version = configuration.ApiVerion
        //        });

        //        c.AddSecurityDefinition(IdentityServerAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        //        {
        //            Type = SecuritySchemeType.OAuth2,
        //            Flows = new OpenApiOAuthFlows
        //            {
        //                Implicit = new OpenApiOAuthFlow
        //                {
        //                    AuthorizationUrl = new Uri($"{configuration.IdentityServerBaseUrl}/connect/authorize"),
        //                    Scopes = new Dictionary<string, string>()
        //                {
        //                    {"tedu_microservices_api.read", "Tedu Microservices API Read"},
        //                    {"tedu_microservices_api.write", "Tedu Microservices API Write"}
        //                }
        //                }
        //            }
        //        });

        //        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //        {
        //            {
        //                new OpenApiSecurityScheme
        //                {
        //                    Reference = new OpenApiReference
        //                    {
        //                        Type = ReferenceType.SecurityScheme,
        //                        Id = IdentityServerAuthenticationDefaults.AuthenticationScheme
        //                    },
        //                    Name = IdentityServerAuthenticationDefaults.AuthenticationScheme
        //                },
        //                new List<string>
        //                {
        //                    "tedu_microservices_api.read",
        //                    "tedu_microservices_api.write"
        //                }
        //            }
        //        });
        //    });
        //}
    }
}
