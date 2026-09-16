using Application.Abstractions;
using Application.DTO.Buildings;
using Application.DTO.Common;
using Application.Services;
using Application.UnitTests.Fakes;
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

            public Task<PagedResult<Building>> GetByClientIdAsync(
                Guid clientId,
                PaginationRequest pagination)
            {
                var all = Storage
                    .Where(b => b.ClientId == clientId)
                    .OrderBy(b => b.Street)
                    .ThenBy(b => b.Number)
                    .ThenBy(b => b.Id)
                    .ToList();
                var pageNumber = Math.Max(pagination.PageNumber, 1);
                var pageSize = Math.Min(Math.Max(pagination.PageSize, 1), 100);

                return Task.FromResult(new PagedResult<Building>
                {
                    Items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = all.Count
                });
            }

            public Task UpdateAsync(Building building)
            {
                // in-memory already updated by reference
                return Task.CompletedTask;
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

            var client = new Domain.Entities.Client(ClientType.Individual, "John", "1234567890123");
            clientRepo.Seed(client);
            var clientId = client.Id;
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "OldSt", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);
            buildingRepo.Storage.Add(building);
            geoRepo.SeedCity(cityId);

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

            var client = new Domain.Entities.Client(ClientType.Individual, "John", "1234567890123");
            clientRepo.Seed(client);
            var clientId = client.Id;
            var cityId = Guid.NewGuid();
            var b1 = new Building(clientId, cityId, "A", "1", 1990, BuildingType.Residential, 1, 50m, 1000m);
            var b2 = new Building(clientId, cityId, "B", "2", 1991, BuildingType.Residential, 2, 75m, 1500m);
            buildingRepo.Storage.Add(b1);
            buildingRepo.Storage.Add(b2);

            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var list = await service.GetByClientIdAsync(
                clientId,
                new PaginationRequest { PageSize = 10 });

            Assert.Equal(2, list.TotalCount);
            Assert.Equal(2, list.Items.Count);
            Assert.Contains(list.Items, x => x.Id == b1.Id);
            Assert.Contains(list.Items, x => x.Id == b2.Id);
        }

        [Fact]
        public async Task GetByClientIdAsync_Should_Return_Empty_When_None()
        {
            var buildingRepo = new FakeBuildingRepository();
            var clientRepo = new FakeClientRepository();
            var geoRepo = new FakeGeographyRepository();

            var client = new Domain.Entities.Client(ClientType.Individual, "John", "1234567890123");
            clientRepo.Seed(client);
            var service = new BuildingService(buildingRepo, clientRepo, geoRepo);

            var list = await service.GetByClientIdAsync(
                client.Id,
                new PaginationRequest());

            Assert.NotNull(list);
            Assert.Empty(list.Items);
            Assert.Equal(0, list.TotalCount);
        }
    }
}
