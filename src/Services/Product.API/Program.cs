using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} Api up");

try
{
    Log.Information($"Start build Service in Program file");
    builder.Host.UseSerilog(Serilogger.Configure);
    Log.Information($"Build success Serilog");

    builder.Host.AddAppConfigurations();
    Log.Information($"Build success AppConfigurations");

    builder.Services.AddInfrastructure(builder.Configuration);
    Log.Information($"Build success Infrastructure");

    // builder.Services.AddConfigurationSettings(builder.Configuration);
    Log.Information($"Build success ConfigurationSettings");

    Log.Information($"End build Service in Program file");
    var app = builder.Build();

    Log.Information($"Start build Pipeline in Program file");
    app.UseInfrastructure();
    app.MapControllers();

    app.MigrateDatabase<ProductContext>((context, _) =>
    {
        ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
    })
        .Run();
    Log.Information($"End build Pipeline in Program file");
}
catch (Exception ex)
{
    Log.Information($"Error Product API :{ex.Message}");
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



//using Common.Logging;
//using Serilog;

//Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
//Log.Information("Starting Product API up");

//try
//{
//    var builder = WebApplication.CreateBuilder(args);

//    builder.Host.UseSerilog(Serilogger.Configure);
//    // Add services to the container.

//    builder.Services.AddControllers();
//    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//    builder.Services.AddEndpointsApiExplorer();
//    builder.Services.AddSwaggerGen();

//    var app = builder.Build();

//    // Configure the HTTP request pipeline.
//    if (app.Environment.IsDevelopment())
//    {
//        app.UseSwagger();
//        app.UseSwaggerUI();
//    }

//   // app.UseHttpsRedirection();

//   // app.UseAuthorization();

//    app.MapControllers();

//    app.Run();
//}
//catch (Exception ex)
//{
//    Log.Fatal(ex, "Unhandlerd exception");
//}
//finally
//{
//    Log.Information("Shut down Product API complete");
//    Log.CloseAndFlush();
//}

