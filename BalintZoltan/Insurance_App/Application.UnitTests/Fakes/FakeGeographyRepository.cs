using Application.Abstractions;
using Domain.Entities;

namespace Application.UnitTests.Fakes;

public sealed class FakeGeographyRepository : IGeographyRepository
{
    private readonly List<Country> _countries = new();
    private readonly List<County> _counties = new();
    private readonly List<City> _cities = new();
    private readonly HashSet<Guid> _existingCityIds = new();

    public void SeedCountry(Country country) => _countries.Add(country);

    public void SeedCounty(County county) => _counties.Add(county);

    public void SeedCity(City city)
    {
        _cities.Add(city);
        _existingCityIds.Add(city.Id);
    }

    public void SeedCity(Guid cityId) => _existingCityIds.Add(cityId);

    public Task<IReadOnlyCollection<Country>> GetCountriesAsync() =>
        Task.FromResult((IReadOnlyCollection<Country>)_countries.ToList());

    public Task<IReadOnlyCollection<County>> GetCountiesByCountryIdAsync(Guid countryId) =>
        Task.FromResult((IReadOnlyCollection<County>)_counties
            .Where(county => county.CountryId == countryId)
            .ToList());

    public Task<IReadOnlyCollection<City>> GetCitiesByCountyIdAsync(Guid countyId) =>
        Task.FromResult((IReadOnlyCollection<City>)_cities
            .Where(city => city.CountyId == countyId)
            .ToList());

    public Task<bool> CityExistsAsync(Guid cityId) =>
        Task.FromResult(_existingCityIds.Contains(cityId));
}
