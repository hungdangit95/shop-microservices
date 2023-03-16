using Ocelot.DependencyInjection;

namespace OcelotApiGw.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        //var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        //services.AddSingleton(jwtSettings);

        //var apiConfigSettings = configuration.GetSection(nameof(ApiConfigurationSettings)).Get<ApiConfigurationSettings>();
        //services.AddSingleton(apiConfigSettings);

        return services;
    }

    public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOcelot(configuration);
        //services.AddJwtAuthentication();
       // services.AddTransient<ITokenService, TokenService>();
        //services.AddSwaggerForOcelot(configuration);
    }
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration["AllowOrigins"];
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(origins)
                .AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    //public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    //{
    //    var settings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
    //    if (settings == null || string.IsNullOrEmpty(settings.Key))
    //        throw new ArgumentNullException($"{nameof(JwtSettings)} is not configured properly");
    //    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
    //    var tokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = signingKey,
    //        ValidateIssuer = false,
    //        ValidateAudience = false,
    //        ValidateLifetime = false,
    //        ClockSkew = TimeSpan.Zero,
    //        RequireExpirationTime = false,
    //    };
    //    services.AddAuthentication(options =>
    //    {
    //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //    }).AddJwtBearer(x =>
    //    {
    //        x.SaveToken = true;
    //        x.RequireHttpsMetadata = false;
    //        x.TokenValidationParameters = tokenValidationParameters;
    //    });
    //    return services;
    //}
}