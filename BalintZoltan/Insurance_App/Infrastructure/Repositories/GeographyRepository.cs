using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class GeographyRepository : IGeographyRepository
{
    private readonly InsuranceDbContext _dbContext;

    public GeographyRepository(InsuranceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Country>> GetCountriesAsync()
    {
        return await _dbContext.Countries
            .AsNoTracking()
            .OrderBy(country => country.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<County>>
        GetCountiesByCountryIdAsync(Guid countryId)
    {
        return await _dbContext.Counties
            .AsNoTracking()
            .Where(county => county.CountryId == countryId)
            .OrderBy(county => county.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<City>>
        GetCitiesByCountyIdAsync(Guid countyId)
    {
        return await _dbContext.Cities
            .AsNoTracking()
            .Where(city => city.CountyId == countyId)
            .OrderBy(city => city.Name)
            .ToListAsync();
    }

    public async Task<bool> CityExistsAsync(Guid cityId)
    {
        return await _dbContext.Cities
            .AsNoTracking()
            .AnyAsync(city => city.Id == cityId);
    }
}