using DemoILogger.Services;
using Serilog;

//create the web application builder and configures services, including built-in ILogger and configuration services
var builder = WebApplication.CreateBuilder(args);

Console.WriteLine(AppContext.BaseDirectory);

//configure Serilog to write logs to a file.
Log.Logger = new LoggerConfiguration()
    //.MinimumLevel.Information()
    //Verbose,Debug,Information,Warning,Error,Fatal
    //.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) //override the minimum level for Microsoft logs to Warning to reduce logs
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File(
        path: Path.Combine(AppContext.BaseDirectory, "logs", "app-.log"),
        rollingInterval: RollingInterval.Day,
        //rollingInterval: RollingInterval.Minute,
        //outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} " + "[{Level:u3}] " + "[{SourceContext}] " + "{Message:lj}{NewLine}{Exception}"
        outputTemplate: 
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} " 
            + "{NewLine}[{Level}] "
            + "{NewLine}[{SourceContext}] "
            //+ "{NewLine}[{Properties}] "
            + "{NewLine}{Message:lj}{NewLine}{Exception}"
            + "{NewLine}"
    //Level:u3 = 3 character level (INF, WRN, ERR, etc.)
    //Level = full level (Information, Warning, Error, etc.)
    //SourceContext = the class that generated the log message
    //Properties = additional properties
    ).CreateLogger();

//connect Ilogger to Serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();



//add services to the DI container
builder.Services.AddControllers(); //support for controllers
builder.Services.AddScoped<AnimalService>(); //registers the service with the dependency injection container, allowing it to be injected into controllers
//builder.Services.AddScoped<ILogger>(); //not need to register ILogger manually, provides ILogger through dependency injection

//AnimalsController -> requests AnimalService -> requests ILogger<AnimalService> ASP.NET Core provides the ILogger<AnimalService> automatically through dependency injection

var app = builder.Build();

app.MapControllers(); //for routes from url requests

app.Run();
