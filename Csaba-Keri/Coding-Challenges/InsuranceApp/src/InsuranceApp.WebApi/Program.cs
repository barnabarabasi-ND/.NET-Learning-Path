using InsuranceApp.Application;
using InsuranceApp.Infrastructure;
using InsuranceApp.WebApi.Errors;
using System.Text.Json.Serialization;

namespace InsuranceApp.WebApi;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("InsuranceDatabase");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'InsuranceDatabase' is missing.");
        }

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(allowIntegerValues: false)
                );
            });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplication();

        builder.Services.AddInfrastructure(connectionString);

        var app = builder.Build();

        app.UseExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}
