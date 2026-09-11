namespace DependencyInjection;

using Application.Abstractions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddDbContext<InsuranceDbContext>(options =>
        {
            options.UseInMemoryDatabase("InsuranceDatabase");
        });

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IBuildingRepository, BuildingRepository>();
        services.AddScoped<IGeographyRepository, GeographyRepository>();

        return services;
    }
}