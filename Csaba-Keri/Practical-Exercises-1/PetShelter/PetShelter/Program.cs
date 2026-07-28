using Microsoft.Extensions.DependencyInjection;

namespace PetShelter;

internal static class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddSingleton<PetShelterApp>();

        /* The using declaration ensures that the ServiceProvider is disposed automatically,
         * allowing the DI container to dispose any disposable services it created.
         * 
         * Here, it doesn't make a practical difference because the registered object graph
         * contains no disposable services.
         */
        using var serviceProvider = services.BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<PetShelterApp>();

        app.Run();
    }
}
