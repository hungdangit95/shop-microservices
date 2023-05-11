using Contracts.Common.Interfaces;
using Contracts.Identity;
using Infrastructure.Common;
using Infrastructures.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repositories;
using Product.API.Repositories.Interfaces;
using Serilog;
using Shared.Configurations;
using System.Text;
using System.Text.Json;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            services.AddSingleton(jwtSettings);

            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            services.AddSingleton(databaseSettings);

            var apiConfigSettings = configuration.GetSection(nameof(ApiConfigurationSettings)).Get<ApiConfigurationSettings>();
            Log.Information($"ApiConfigurationSettings data: {JsonSerializer.Serialize(apiConfigSettings)}");
            services.AddSingleton(apiConfigSettings);


            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false
            };
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParameters;
            });
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureSwagger();

            services.ConfigureProductDbContext(configuration);
            services.AddInfrastructureService();
            services.AddAutoMapper(config => config.AddProfile(new MappingProfile()));
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
            services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBase<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped<IProductRepository, ProductRepository>()
                .AddTransient<ITokenService, TokenService>();
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

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
             {
                 opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                 opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                 {
                     In = ParameterLocation.Header,
                     Description = "Please enter token",
                     Name = "Authorization",
                     Type = SecuritySchemeType.Http,
                     BearerFormat = "JWT",
                     Scheme = "bearer"
                 });
                 opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             }
                         },
                         new string[] { }
                     }
                 });
             });
        }
    }
}
