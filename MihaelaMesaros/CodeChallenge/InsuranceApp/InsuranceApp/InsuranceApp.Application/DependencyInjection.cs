using InsuranceApp.Application.Abstractions.Services;
using InsuranceApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGeographyService, GeographyService>();

        services.AddScoped<IClientService, ClientService>();

        return services;
    }
}