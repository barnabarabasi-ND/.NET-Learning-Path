using System;
using Domain.Entities;
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
    }
}
