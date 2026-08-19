using Microsoft.Extensions.Logging;
using Serilog;
using VariousDemos.Demos;

// Configure Serilog with file and console sinks
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    //.WriteTo.Console()
    .WriteTo.File(
        path: Path.Combine(AppContext.BaseDirectory, "logs", "app-.log"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

// Set up logging factory with Serilog
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddSerilog(dispose: true);
});

var logger = loggerFactory.CreateLogger<DemoFileStreamAsync>();


//=======================================================
//AdvancedTopics.Run();

//LambdaLinqFunctionalProgramming.Run();

//ReflectionDynamicProgramming.Run();

//ResourceManagement.Run();

//MultithreadingAsyncronous.Run();

//FilesStreamsSerialization.Run();
//await FilesStreamsSerialization.Run();

await DemoFileStreamAsync.Run(logger);