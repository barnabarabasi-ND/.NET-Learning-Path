using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Services;
using Domain.Entities;
using Xunit;

namespace Application.UnitTests.Services
{
    public class GeographyServiceTest
    {
        private class FakeGeographyRepository : IGeographyRepository
        {
            private readonly List<Country> _countries = new();
            private readonly List<County> _counties = new();
            private readonly List<City> _cities = new();

            public void SeedCountry(Country country) => _countries.Add(country);
            public void SeedCounty(County county) => _counties.Add(county);
            public void SeedCity(City city) => _cities.Add(city);

            public Task<IReadOnlyCollection<Country>> GetCountriesAsync()
            {
                return Task.FromResult((IReadOnlyCollection<Country>)_countries.ToList());
            }

            public Task<IReadOnlyCollection<County>> GetCountiesByCountryIdAsync(Guid countryId)
            {
                var list = _counties.Where(c => c.CountryId == countryId).ToList();
                return Task.FromResult((IReadOnlyCollection<County>)list);
            }

            public Task<IReadOnlyCollection<City>> GetCitiesByCountyIdAsync(Guid countyId)
            {
                var list = _cities.Where(c => c.CountyId == countyId).ToList();
                return Task.FromResult((IReadOnlyCollection<City>)list);
            }

            public Task<bool> CityExistsAsync(Guid cityId)
            {
                return Task.FromResult(_cities.Any(c => c.Id == cityId));
            }
        }

        [Fact]
        public async Task GetCountriesAsync_Should_Return_List()
        {
            var repo = new FakeGeographyRepository();
            var c1 = new Country("CountryA");
            var c2 = new Country("CountryB");
            repo.SeedCountry(c1);
            repo.SeedCountry(c2);

            var service = new GeographyService(repo);

            var list = await service.GetCountriesAsync();

            Assert.Equal(2, list.Count);
            Assert.Contains(list, x => x.Id == c1.Id && x.Name == "CountryA");
            Assert.Contains(list, x => x.Id == c2.Id && x.Name == "CountryB");
        }

        [Fact]
        public async Task GetCountriesAsync_Should_Return_Empty_When_None()
        {
            var repo = new FakeGeographyRepository();
            var service = new GeographyService(repo);

            var list = await service.GetCountriesAsync();

            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public async Task GetCountiesByCountryIdAsync_Should_Return_Counties_For_Country()
        {
            var repo = new FakeGeographyRepository();
            var country = new Country("Cty");
            var county1 = new County(country.Id, "County1");
            var county2 = new County(country.Id, "County2");
            var other = new County(Guid.NewGuid(), "Other");
            repo.SeedCounty(county1);
            repo.SeedCounty(county2);
            repo.SeedCounty(other);

            var service = new GeographyService(repo);

            var list = await service.GetCountiesByCountryIdAsync(country.Id);

            Assert.Equal(2, list.Count);
            Assert.All(list, c => Assert.Equal(country.Id, c.CountryId));
        }

        [Fact]
        public async Task GetCountiesByCountryIdAsync_Should_Return_Empty_When_None()
        {
            var repo = new FakeGeographyRepository();
            var service = new GeographyService(repo);

            var list = await service.GetCountiesByCountryIdAsync(Guid.NewGuid());

            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public async Task GetCitiesByCountyIdAsync_Should_Return_Cities_For_County()
        {
            var repo = new FakeGeographyRepository();
            var county = new County(Guid.NewGuid(), "Cnty");
            var city1 = new City(county.Id, "City1", "1111");
            var city2 = new City(county.Id, "City2", "2222");
            var other = new City(Guid.NewGuid(), "Other", "3333");
            repo.SeedCity(city1);
            repo.SeedCity(city2);
            repo.SeedCity(other);

            var service = new GeographyService(repo);

            var list = await service.GetCitiesByCountyIdAsync(county.Id);

            Assert.Equal(2, list.Count);
            Assert.All(list, c => Assert.Equal(county.Id, c.CountyId));
        }

        [Fact]
        public async Task GetCitiesByCountyIdAsync_Should_Return_Empty_When_None()
        {
            var repo = new FakeGeographyRepository();
            var service = new GeographyService(repo);

            var list = await service.GetCitiesByCountyIdAsync(Guid.NewGuid());

            Assert.NotNull(list);
            Assert.Empty(list);
        }
    }
}
