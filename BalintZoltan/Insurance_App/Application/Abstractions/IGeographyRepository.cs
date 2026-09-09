namespace Application.Abstractions;

using Application.DTO.Geography;

using Domain.Entities;

public interface IGeographyRepository
{
    Task<IReadOnlyCollection<Country>> GetCountriesAsync();

    Task<IReadOnlyCollection<County>> GetCountiesByCountryIdAsync(
        Guid countryId);

    Task<IReadOnlyCollection<City>> GetCitiesByCountyIdAsync(
        Guid countyId);
}