namespace Infrastructure.Seed;

using System.Text.Json;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class GeographySeeder
{
    public static async Task SeedAsync(
        InsuranceDbContext dbContext,
        string contentRootPath,
        CancellationToken cancellationToken = default)
    {
        if (await dbContext.Countries.AnyAsync(cancellationToken))
        {
            return;
        }

        var filePath = Path.GetFullPath(Path.Combine(
            contentRootPath,
            "..",
            "resources",
            "Geography_Data.json"));

        await using var stream = File.OpenRead(filePath);

        var geographyData = await JsonSerializer.DeserializeAsync<
            Dictionary<string, Dictionary<string, string>>>(
                stream,
                cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException(
                "The Geography_Data.json file is empty or invalid.");

        var country = new Country("Romania");
        dbContext.Countries.Add(country);

        foreach (var countyData in geographyData)
        {
            var county = new County(country.Id, countyData.Key);
            dbContext.Counties.Add(county);

            foreach (var cityData in countyData.Value)
            {
                dbContext.Cities.Add(new City(
                    county.Id,
                    cityData.Key,
                    cityData.Value));
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
