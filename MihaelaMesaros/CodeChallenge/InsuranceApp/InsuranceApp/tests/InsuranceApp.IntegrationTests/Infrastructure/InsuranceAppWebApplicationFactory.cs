using InsuranceApp.Domain.Entities;
using InsuranceApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InsuranceApp.IntegrationTests.Infrastructure;

public sealed class InsuranceAppWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;

    public InsuranceAppWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<InsuranceDbContext>();
            services.RemoveAll<DbContextOptions<InsuranceDbContext>>();

            services.AddDbContext<InsuranceDbContext>(options =>
                options.UseSqlite(_connection));

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<InsuranceDbContext>();

            dbContext.Database.EnsureCreated();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        // Delete children before parents to respect foreign keys.

        dbContext.Policies.RemoveRange(
            dbContext.Policies);

        dbContext.Buildings.RemoveRange(
            dbContext.Buildings);

        dbContext.RiskFactorConfigs.RemoveRange(
            dbContext.RiskFactorConfigs);

        dbContext.Clients.RemoveRange(
            dbContext.Clients);

        dbContext.Cities.RemoveRange(
            dbContext.Cities);

        dbContext.Counties.RemoveRange(
            dbContext.Counties);

        dbContext.Countries.RemoveRange(
            dbContext.Countries);

        dbContext.BuildingTypes.RemoveRange(
            dbContext.BuildingTypes);

        dbContext.Currencies.RemoveRange(
            dbContext.Currencies);

        dbContext.FeeConfigs.RemoveRange(
            dbContext.FeeConfigs);

        dbContext.Brokers.RemoveRange(
            dbContext.Brokers);

        await dbContext.SaveChangesAsync();
    }

    public async Task SeedGeographyAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var romania = new Country
        {
            CountryId = Guid.NewGuid(),
            Name = "Romania"
        };

        var hungary = new Country
        {
            CountryId = Guid.NewGuid(),
            Name = "Hungary"
        };

        var cluj = new County
        {
            CountyId = Guid.NewGuid(),
            CountryId = romania.CountryId,
            Name = "Cluj"
        };

        var brasov = new County
        {
            CountyId = Guid.NewGuid(),
            CountryId = romania.CountryId,
            Name = "Brasov"
        };

        var clujNapoca = new City
        {
            CityId = Guid.NewGuid(),
            CountyId = cluj.CountyId,
            Name = "Cluj-Napoca"
        };

        var turda = new City
        {
            CityId = Guid.NewGuid(),
            CountyId = cluj.CountyId,
            Name = "Turda"
        };

        dbContext.Countries.AddRange(
            romania,
            hungary);

        dbContext.Counties.AddRange(
            cluj,
            brasov);

        dbContext.Cities.AddRange(
            clujNapoca,
            turda);

        await dbContext.SaveChangesAsync();
    }

    public async Task SeedBuildingTypesAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        if (await dbContext.BuildingTypes.AnyAsync())
        {
            return;
        }

        dbContext.BuildingTypes.AddRange(
            new BuildingType
            {
                BuildingTypeId = Guid.NewGuid(),
                Name = "Residential"
            },
            new BuildingType
            {
                BuildingTypeId = Guid.NewGuid(),
                Name = "Office"
            },
            new BuildingType
            {
                BuildingTypeId = Guid.NewGuid(),
                Name = "Industrial"
            });

        await dbContext.SaveChangesAsync();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }
}
