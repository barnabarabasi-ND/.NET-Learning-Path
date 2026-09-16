using InsuranceApp.Application;
using InsuranceApp.Infrastructure;

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

        builder.Services.AddApplication();

        builder.Services.AddInfrastructure(connectionString);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapOpenApi();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
