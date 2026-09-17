using Application.Abstractions;
using Application.DTO.Geography;
using Domain.Entities;

namespace Application.Services;

public class GeographyService : IGeographyService
{
    private readonly IGeographyRepository _geographyRepository;

    public GeographyService(IGeographyRepository geographyRepository)
    {
        _geographyRepository = geographyRepository;
    }

    public async Task<IReadOnlyCollection<CountryDto>> GetCountriesAsync()
    {
        var countries = await _geographyRepository.GetCountriesAsync();

        return countries
            .Select(MapToCountryDto)
            .ToList();
    }

    public async Task<IReadOnlyCollection<CountyDto>> GetCountiesByCountryIdAsync(
        Guid countryId)
    {
        var counties = await _geographyRepository
            .GetCountiesByCountryIdAsync(countryId);

        return counties
            .Select(MapToCountyDto)
            .ToList();
    }

    public async Task<IReadOnlyCollection<CityDto>> GetCitiesByCountyIdAsync(
        Guid countyId)
    {
        var cities = await _geographyRepository
            .GetCitiesByCountyIdAsync(countyId);

        return cities
            .Select(MapToCityDto)
            .ToList();
    }

    private static CountryDto MapToCountryDto(Country country)
    {
        return new CountryDto
        {
            Id = country.Id,
            Name = country.Name
        };
    }

    private static CountyDto MapToCountyDto(County county)
    {
        return new CountyDto
        {
            Id = county.Id,
            CountryId = county.CountryId,
            Name = county.Name
        };
    }

    private static CityDto MapToCityDto(City city)
    {
        return new CityDto
        {
            Id = city.Id,
            CountyId = city.CountyId,
            Name = city.Name,
            PostalCode = city.PostalCode
        };
    }
}