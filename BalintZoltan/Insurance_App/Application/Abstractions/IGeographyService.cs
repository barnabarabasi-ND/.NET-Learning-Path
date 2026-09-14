using Application.DTO.Geography;

namespace Application.Abstractions;

public interface IGeographyService
{
    Task<IReadOnlyCollection<CountryDto>> GetCountriesAsync();

    Task<IReadOnlyCollection<CountyDto>> GetCountiesByCountryIdAsync(
        Guid countryId);

    Task<IReadOnlyCollection<CityDto>> GetCitiesByCountyIdAsync(
        Guid countyId);
}