using Domain.Entities;
using Domain.Enums;
using System;
using Xunit;

namespace Domain.UnitTests.Entities
{
    public class CityTest
    {
        [Fact]
        public void Create_Should_Set_Properties()
        {
            var countyId = Guid.NewGuid();
            var city = new City(countyId, "Sample City", "12345");

            Assert.NotEqual(Guid.Empty, city.Id);
            Assert.Equal(countyId, city.CountyId);
            Assert.Equal("Sample City", city.Name);
            Assert.Equal("12345", city.PostalCode);
        }
        [Fact]
        public void Create_Should_Throw_When_CountyId_Empty()
        {
            var countyId = Guid.Empty;
            Assert.Throws<ArgumentException>(() => new City(countyId, "Sample City", ""));
        }
        [Fact]
        public void Create_Should_Throw_When_Name_Empty()
        {
            var countyId = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() => new City(countyId, "", "12345"));
        }
        [Fact]
        public void Create_Should_Throw_When_PostalCode_Empty()
        {
            var countyId = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() => new City(countyId, "Sample City", ""));
        }
    }
}
