namespace Application.Services;

using Application.Abstractions;
using Application.DTO.Geography;
using Domain.Entities;

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
            .Select(MapToDto)
            .ToList();
    }

    public async Task<IReadOnlyCollection<CountyDto>> GetCountiesByCountryIdAsync(
        Guid countryId)
    {
        var counties = await _geographyRepository
            .GetCountiesByCountryIdAsync(countryId);

        return counties
            .Select(MapToDto)
            .ToList();
    }

    public async Task<IReadOnlyCollection<CityDto>> GetCitiesByCountyIdAsync(
        Guid countyId)
    {
        var cities = await _geographyRepository
            .GetCitiesByCountyIdAsync(countyId);

        return cities
            .Select(MapToDto)
            .ToList();
    }

    private static CountryDto MapToDto(Country country)
    {
        return new CountryDto
        {
            Id = country.Id,
            Name = country.Name
        };
    }

    private static CountyDto MapToDto(County county)
    {
        return new CountyDto
        {
            Id = county.Id,
            CountryId = county.CountryId,
            Name = county.Name
        };
    }

    private static CityDto MapToDto(City city)
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