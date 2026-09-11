using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.DTO.Buildings;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Xunit;

namespace Application.UnitTests.Services
{
    public class BuildingServiceTest
    {
        private class FakeBuildingRepository : IBuildingRepository
        {
            public readonly List<Building> Storage = new();
            public Task AddAsync(Building building)
            {
                Storage.Add(building);
                return Task.CompletedTask;
            }

            public Task<Building?> GetByIdAsync(Guid id)
            {
                return Task.FromResult(Storage.FirstOrDefault(b => b.Id == id));
            }

            public Task<IReadOnlyCollection<Building>> GetByClientIdAsync(Guid clientId)
            {
                return Task.FromResult((IReadOnlyCollection<Building>)Storage.Where(b => b.ClientId == clientId).ToList());
            }

            public Task UpdateAsync(Building building)
            {
                // in-memory already updated by reference
                return Task.CompletedTask;
            }
        }

        private class FakeClientRepository : IClientRepository
        {
            private readonly Dictionary<Guid, Client> _clients = new();

            public void Seed(Client client) => _clients[client.Id] = client;

            public Task AddAsync(Client client)
            {
                _clients[client.Id] = client;
                return Task.CompletedTask;
            }

            public Task<bool> ExistsByIdentificationNumberAsync(string identificationNumber, Guid? excludedClientId = null)
            {
                var exists = _clients.Values.Any(c => c.IdentificationNumber == identificationNumber && c.Id != excludedClientId);
                return Task.FromResult(exists);
            }

            public Task<Client?> GetByIdAsync(Guid id)
            {
                _clients.TryGetValue(id, out var client);
                return Task.FromResult(client);
            }

            public Task<IReadOnlyCollection<Client>> SearchAsync(string? searchTerm)
            {
                return Task.FromResult((IReadOnlyCollection<Client>)_clients.Values.ToList());
            }

            public Task UpdateAsync(Client client)
            {
                _clients[client.Id] = client;
                return Task.CompletedTask;
            }
        }

        private class FakeGeographyRepository : IGeographyRepository
        {
            private readonly HashSet<Guid> _cities = new();

            public void SeedCity(Guid id) => _cities.Add(id);

            public Task<IReadOnlyCollection<Domain.Entities.Country>> GetCountriesAsync()
            {
                return Task.FromResult((IReadOnlyCollection<Domain.Entities.Country>)Array.Empty<Domain.Entities.Country>());
            }

            public Task<IReadOnlyCollection<Domain.Entities.County>> GetCountiesByCountryIdAsync(Guid countryId)
            {
                return Task.FromResult((IReadOnlyCollection<Domain.Entities.County>)Array.Empty<Domain.Entities.County>());
            }

            public Task<IReadOnlyCollection<Domain.Entities.City>> GetCitiesByCountyIdAsync(Guid countyId)
            {
                return Task.FromResult((IReadOnlyCollection<Domain.Entities.City>)Array.Empty<Domain.Entities.City>());
            }

            public Task<bool> CityExistsAsync(Guid cityId)
            {
                return Task.FromResult(_cities.Contains(cityId));
            }
        }

        [Fact]
        public async Task CreateAsync_Should_Create_When_Client_And_City_Exist()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var existingClient = new Domain.Entities.Client(ClientType.Individual, "John", "ID1");
            clientRepo.Seed(existingClient);

            var cityId = Guid.NewGuid();
            geoRepo.SeedCity(cityId);

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var request = new CreateBuildingRequest
            {
                ClientId = existingClient.Id,
                CityId = cityId,
                Street = "Main",
                Number = "1",
                ConstructionYear = 2000,
                Type = BuildingType.Residential,
                NumberOfFloors = 2,
                SurfaceArea = 100m,
                InsuredValue = 10000m,
                IsFloodRiskZone = false,
                IsEarthquakeRiskZone = true
            };

            var dto = await service.CreateAsync(request);

            Assert.NotNull(dto);
            Assert.Equal(request.ClientId, dto.ClientId);
            Assert.Equal(request.CityId, dto.CityId);
            Assert.Equal(request.Street, dto.Street);
            Assert.Equal(request.Number, dto.Number);
            Assert.Equal(request.SurfaceArea, dto.SurfaceArea);
            Assert.Equal(request.IsEarthquakeRiskZone, dto.IsEarthquakeRiskZone);
            // repository should have stored one building
            Assert.Single(buildingRepo.Storage);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Client_Not_Found()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var request = new CreateBuildingRequest
            {
                ClientId = Guid.NewGuid(),
                CityId = Guid.NewGuid(),
                Street = "Main",
                Number = "1",
                ConstructionYear = 2000,
                Type = BuildingType.Residential,
                NumberOfFloors = 2,
                SurfaceArea = 100m,
                InsuredValue = 10000m,
                IsFloodRiskZone = false,
                IsEarthquakeRiskZone = true
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));
            Assert.Equal("Client was not found.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_City_Not_Found()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var existingClient = new Domain.Entities.Client(ClientType.Individual, "John", "ID1");
            clientRepo.Seed(existingClient);

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var request = new CreateBuildingRequest
            {
                ClientId = existingClient.Id,
                CityId = Guid.NewGuid(),
                Street = "Main",
                Number = "1",
                ConstructionYear = 2000,
                Type = BuildingType.Residential,
                NumberOfFloors = 2,
                SurfaceArea = 100m,
                InsuredValue = 10000m,
                IsFloodRiskZone = false,
                IsEarthquakeRiskZone = true
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));
            Assert.Equal("City was not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_When_Building_Exists()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "OldSt", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);
            buildingRepo.Storage.Add(building);

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var update = new UpdateBuildingRequest
            {
                CityId = cityId,
                Street = "NewSt",
                Number = "2",
                ConstructionYear = 2001,
                Type = BuildingType.Residential,
                NumberOfFloors = 3,
                SurfaceArea = 200m,
                InsuredValue = 5000m,
                IsFloodRiskZone = true,
                IsEarthquakeRiskZone = false
            };

            var dto = await service.UpdateAsync(building.Id, update);

            Assert.Equal(building.Id, dto.Id);
            Assert.Equal(update.Street, dto.Street);
            Assert.Equal(update.Number, dto.Number);
            Assert.Equal(update.NumberOfFloors, dto.NumberOfFloors);
            Assert.Equal(update.SurfaceArea, dto.SurfaceArea);
            Assert.Equal(update.IsFloodRiskZone, dto.IsFloodRiskZone);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Building_Not_Found()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var update = new UpdateBuildingRequest
            {
                CityId = Guid.NewGuid(),
                Street = "NewSt",
                Number = "2",
                ConstructionYear = 2001,
                Type = BuildingType.Residential,
                NumberOfFloors = 3,
                SurfaceArea = 200m,
                InsuredValue = 5000m,
                IsFloodRiskZone = true,
                IsEarthquakeRiskZone = false
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), update));
            Assert.Equal("Building was not found.", ex.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Dto_When_Building_Exists()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1995, BuildingType.Residential, 2, 80m, 2000m);
            buildingRepo.Storage.Add(building);

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var dto = await service.GetByIdAsync(building.Id);

            Assert.NotNull(dto);
            Assert.Equal(building.Id, dto!.Id);
            Assert.Equal(building.Street, dto.Street);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var dto = await service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(dto);
        }

        [Fact]
        public async Task GetByClientIdAsync_Should_Return_Buildings_For_Client()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var b1 = new Building(clientId, cityId, "A", "1", 1990, BuildingType.Residential, 1, 50m, 1000m);
            var b2 = new Building(clientId, cityId, "B", "2", 1991, BuildingType.Residential, 2, 75m, 1500m);
            buildingRepo.Storage.Add(b1);
            buildingRepo.Storage.Add(b2);

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var list = await service.GetByClientIdAsync(clientId);

            Assert.Equal(2, list.Count);
            Assert.Contains(list, x => x.Id == b1.Id);
            Assert.Contains(list, x => x.Id == b2.Id);
        }

        [Fact]
        public async Task GetByClientIdAsync_Should_Return_Empty_When_None()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var list = await service.GetByClientIdAsync(Guid.NewGuid());

            Assert.NotNull(list);
            Assert.Empty(list);
        }
    }
}
