using Microsoft.Extensions.DependencyInjection;

namespace VehicleManagement;

internal static class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddSingleton<VehicleManagementApp>();

        /* The using declaration ensures that the ServiceProvider is disposed automatically,
         * allowing the DI container to dispose any disposable services it created.
         * 
         * Here, it doesn't make a practical difference because the registered object graph
         * contains no disposable services.
         */
        using var serviceProvider = services.BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<VehicleManagementApp>();
        
        app.Run();
    }
}
