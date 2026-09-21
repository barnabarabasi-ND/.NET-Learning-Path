using InsuranceApp.Application.DTOs.Geography;
using InsuranceApp.IntegrationTests.Common;
using InsuranceApp.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace InsuranceApp.IntegrationTests.Api.Controllers.Broker;

public sealed class GeographyEndpointsTests(InsuranceAppWebApplicationFactory factory) : IClassFixture<InsuranceAppWebApplicationFactory>, IAsyncLifetime
{
    private readonly InsuranceAppWebApplicationFactory _factory = factory;
    private readonly HttpClient _client = factory.CreateClient();

    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
        await _factory.SeedGeographyAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;


    [Fact]
    public async Task GetCountries_ReturnsOkWithCountries()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/brokers/countries");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var countries =
            await response.Content
                .ReadFromJsonAsync<List<CountryDto>>();

        Assert.NotNull(countries);
        Assert.Equal(2, countries.Count);

        Assert.Contains(
            countries,
            x => x.Name == "Romania");

        Assert.Contains(
            countries,
            x => x.Name == "Hungary");
    }

    [Fact]
    public async Task GetCounties_WhenCountryExists_ReturnsOkWithCounties()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/brokers/countries/1/counties");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var counties =
            await response.Content
                .ReadFromJsonAsync<List<CountyDto>>();

        Assert.NotNull(counties);
        Assert.Equal(2, counties.Count);

        Assert.Contains(
            counties,
            x => x.Name == "Cluj");

        Assert.Contains(
            counties,
            x => x.Name == "Brasov");
    }

    [Fact]
    public async Task GetCounties_WhenCountryDoesNotExist_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/brokers/countries/{TestConstants.NonExistingId}/counties");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetCities_WhenCountyExists_ReturnsOkWithCities()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/brokers/counties/1/cities");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var cities =
            await response.Content
                .ReadFromJsonAsync<List<CityDto>>();

        Assert.NotNull(cities);
        Assert.Equal(2, cities.Count);

        Assert.Contains(
            cities,
            x => x.Name == "Cluj-Napoca");

        Assert.Contains(
            cities,
            x => x.Name == "Turda");
    }

    [Fact]
    public async Task GetCities_WhenCountyDoesNotExist_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/brokers/counties/{TestConstants.NonExistingId}/cities");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetCounties_WhenCountryDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/brokers/countries/{TestConstants.NonExistingId}/counties");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);

        var problem =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
        Assert.Equal("Resource not found", problem.Title);
        Assert.NotNull(problem.Detail);
    }

}