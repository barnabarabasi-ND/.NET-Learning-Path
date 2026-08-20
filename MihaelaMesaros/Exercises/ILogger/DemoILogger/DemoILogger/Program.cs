using DemoILogger.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine(AppContext.BaseDirectory);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File(
        path: Path.Combine(AppContext.BaseDirectory, "logs", "app-.log"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: 
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} " 
            + "{NewLine}[{Level}] "
            + "{NewLine}[{SourceContext}] "
            + "{NewLine}{Message:lj}{NewLine}{Exception}"
            + "{NewLine}"
    ).CreateLogger();

//connect Ilogger to Serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();


//add services to the DI container
builder.Services.AddControllers();
builder.Services.AddScoped<AnimalService>();

var app = builder.Build();

app.MapControllers();

app.Run();
