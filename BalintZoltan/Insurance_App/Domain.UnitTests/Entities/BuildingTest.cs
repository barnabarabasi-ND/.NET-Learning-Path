using Domain.Entities;
using Domain.Enums;
using Xunit;

namespace Domain.UnitTests.Entities
{
    public class BuildingTest
    {
        [Fact]
        public void Create_With_Valid_Data_Should_Succeed()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            var building = new Building(
                clientId,
                cityId,
                "Main St",
                "10A",
                2000,
                BuildingType.Residential,
                3,
                120.5m,
                250000m,
                isFloodRiskZone: true,
                isEarthquakeRiskZone: false);

            Assert.NotEqual(Guid.Empty, building.Id);
            Assert.Equal(clientId, building.ClientId);
            Assert.Equal(cityId, building.CityId);
            Assert.Equal("Main St", building.Street);
            Assert.Equal("10A", building.Number);
            Assert.Equal(2000, building.ConstructionYear);
            Assert.Equal(BuildingType.Residential, building.Type);
            Assert.Equal(3, building.NumberOfFloors);
            Assert.Equal(120.5m, building.SurfaceArea);
            Assert.Equal(250000m, building.InsuredValue);
            Assert.True(building.IsFloodRiskZone);
            Assert.False(building.IsEarthquakeRiskZone);
        }

        [Fact]
        public void Constructor_Should_Throw_When_ClientId_Empty()
        {
            var cityId = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() =>
                new Building(Guid.Empty, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m));
        }

        [Fact]
        public void Constructor_Should_Throw_When_CityId_Empty()
        {
            var cityId = Guid.Empty;
            Assert.Throws<ArgumentException>(() =>
                new Building(Guid.NewGuid(), cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m));
        }

        [Fact]
        public void Constructor_Should_Throw_When_Street_Empty()
        {
            var street = String.Empty;
            Assert.Throws<ArgumentException>(() =>
                new Building(Guid.NewGuid(), Guid.NewGuid(), street, "1", 1990, BuildingType.Administrative, 1, 50m, 1000m));
        }


        [Fact]
        public void Constructor_Should_Throw_When_SurfaceArea_NonPositive()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() =>
                new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 0m, 1000m));
        }

        [Fact]
        public void UpdateAddress_Should_Succeed()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            building.UpdateAddress(cityId, "New St", "2");
            Assert.Equal(cityId, building.CityId);
            Assert.Equal("New St", building.Street);
            Assert.Equal("2", building.Number);
        }

        [Fact]
        public void UpdateAddress_Should_Throw_When_CityId_Empty()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            Assert.Throws<ArgumentException>(() => building.UpdateAddress(Guid.Empty, "New St", "2"));
        }

        [Fact]
        public void UpdateAddress_Should_Throw_When_Street_Empty()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var street = String.Empty;
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            Assert.Throws<ArgumentException>(() => building.UpdateAddress(cityId, street, "2"));
        }

        [Fact]
        public void UpdateAddress_Should_Throw_When_Nr_Empty()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var nr = String.Empty;
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            Assert.Throws<ArgumentException>(() => building.UpdateAddress(cityId, "New St", nr));
        }

        [Fact]
        public void UpdateDetails_Should_Succeed()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            building.UpdateDetails(2000, BuildingType.Residential, 3, 120.5m, 250000m);

            Assert.Equal(2000, building.ConstructionYear);
            Assert.Equal(BuildingType.Residential, building.Type);
            Assert.Equal(3, building.NumberOfFloors);
            Assert.Equal(120.5m, building.SurfaceArea);
            Assert.Equal(250000m, building.InsuredValue);
        }

        [Fact]
        public void UpdateDetails_Should_Throw_When_InsuredValue_NonPositive()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            Assert.Throws<ArgumentException>(() => building.UpdateDetails(1991, BuildingType.Residential, 2, 100m, 0m));
        }

        [Fact]
        public void UpdateDetails_Should_Throw_When_SurfaceArea_NonPositive()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            Assert.Throws<ArgumentException>(() => building.UpdateDetails(1991, BuildingType.Residential, 2, 0m, 1100m));
        }

        [Fact]
        public void UpdateDetails_Should_Throw_When_NumberOfFloors_NonPositive()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m);

            Assert.Throws<ArgumentException>(() => building.UpdateDetails(1991, BuildingType.Residential, -1, 50m, 1100m));
        }

        [Fact]
        public void UpdateRiskIndicators_Should_Update_Flags()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 1, 50m, 1000m, isFloodRiskZone: false, isEarthquakeRiskZone: false);

            building.UpdateRiskIndicators(true, true);

            Assert.True(building.IsFloodRiskZone);
            Assert.True(building.IsEarthquakeRiskZone);
        }

        [Fact]
        public void UpdateRiskIndicators_Should_Not_Modify_Other_Properties()
        {
            var clientId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var building = new Building(clientId, cityId, "St", "1", 1990, BuildingType.Administrative, 2, 75m, 1500m, isFloodRiskZone: false, isEarthquakeRiskZone: false);

            var beforeFloors = building.NumberOfFloors;
            var beforeSurface = building.SurfaceArea;
            var beforeInsured = building.InsuredValue;

            building.UpdateRiskIndicators(true, false);

            Assert.Equal(beforeFloors, building.NumberOfFloors);
            Assert.Equal(beforeSurface, building.SurfaceArea);
            Assert.Equal(beforeInsured, building.InsuredValue);
        }
    }
}
