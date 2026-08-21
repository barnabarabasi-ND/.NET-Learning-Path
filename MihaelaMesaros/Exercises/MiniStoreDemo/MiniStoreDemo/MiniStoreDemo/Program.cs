using MiniStoreDemo.Data;
using MiniStoreDemo.Repositories;
using MiniStoreDemo.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

//register services which are necessary to generate API documentation
builder.Services.AddOpenApi();


//configure log file in BaseDirectory
string logFolder = builder.Configuration["LoggingSettings:LogFolder"] ?? throw new InvalidOperationException("LoggingSettings > LogFolder is not configured.");
string logFileNameInfo = builder.Configuration["LoggingSettings:LogFileNameInfo"] ?? throw new InvalidOperationException("LoggingSettings > LogFileNameInfo is not configured.");
string logFileNameError = builder.Configuration["LoggingSettings:LogFileNameError"] ?? throw new InvalidOperationException("LoggingSettings > LogFileNameError is not configured.");
//string logFileNameAudit = builder.Configuration["LoggingSettings:LogFileNameAudit"] ?? throw new InvalidOperationException("LoggingSettings > LogFileNameAudit is not configured.");

string logPathInfo = Path.Combine(AppContext.BaseDirectory, logFolder, logFileNameInfo);
string logPathError = Path.Combine(AppContext.BaseDirectory, logFolder, logFileNameError);
//string logPathAudit = Path.Combine(AppContext.BaseDirectory, logFolder, logFileNameAudit);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        //.WriteTo.File(path: logPath, rollingInterval: RollingInterval.Day)
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(logEvent => logEvent.Level >= LogEventLevel.Information && logEvent.Level < LogEventLevel.Error)
            .WriteTo.File(
                logPathInfo,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
        )
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(logEvent => logEvent.Level >= LogEventLevel.Error)
            .WriteTo.File(
                logPathError,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 90));
});

var app = builder.Build();

//log HTTP requests and responses
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //exposes the generated OpenAPI document through an endpoint
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
