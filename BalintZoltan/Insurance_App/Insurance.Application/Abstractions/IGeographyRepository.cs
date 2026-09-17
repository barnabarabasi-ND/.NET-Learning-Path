using Domain.Entities;

namespace Application.Abstractions;

public interface IGeographyRepository
{
    Task<IReadOnlyCollection<Country>> GetCountriesAsync();

    Task<IReadOnlyCollection<County>> GetCountiesByCountryIdAsync(
        Guid countryId);

    Task<IReadOnlyCollection<City>> GetCitiesByCountyIdAsync(
        Guid countyId);

    Task<bool> CityExistsAsync(Guid cityId);
}