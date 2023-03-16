using Common.Logging;
using OcelotApiGw.Extensions;
using Serilog;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} Api up");

try
{
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureOcelot(builder.Configuration);
    builder.Services.ConfigureCors(builder.Configuration);
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseCors("CorsPolicy");
   // app.UseMiddleware<ErrorWrappingMiddleware>();
    // app.UseAuthentication();
    app.UseRouting();
    ///app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", context =>
        {
            //await context.Response.WriteAsync($"Hello hello");
            context.Response.Redirect("swagger/index.html");
            return Task.CompletedTask;
        });
    });
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
        throw;
    Log.Fatal(ex, "Unhanded exception");
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}


