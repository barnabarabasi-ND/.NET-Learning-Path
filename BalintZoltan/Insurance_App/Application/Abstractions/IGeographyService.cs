namespace Application.Abstractions;

using Application.DTO.Geography;

public interface IGeographyService
{
    Task<IReadOnlyCollection<CountryDto>> GetCountriesAsync();

    Task<IReadOnlyCollection<CountyDto>> GetCountiesByCountryIdAsync(
        Guid countryId);

    Task<IReadOnlyCollection<CityDto>> GetCitiesByCountyIdAsync(
        Guid countyId);
}