namespace InsuranceApp.WebApi;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapOpenApi();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
