using System.Net;
using System.Net.Http.Json;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Domain.Entities;
using InsuranceApp.Infrastructure.Persistence;
using InsuranceApp.IntegrationTests.Common;
using InsuranceApp.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.IntegrationTests.Api.Controllers.Admin;

public sealed class CurrencyEndpointsTests(InsuranceAppWebApplicationFactory factory)
    : IClassFixture<InsuranceAppWebApplicationFactory>, IAsyncLifetime
{
    private readonly InsuranceAppWebApplicationFactory _factory = factory;
    private readonly HttpClient _client = factory.CreateClient();

    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;


    #region Create Currency Tests

    [Fact]
    public async Task CreateCurrency_ValidRequest_ReturnsCreatedAndPersistsCurrency()
    {
        // Arrange
        var request = CreateValidCurrencyDto();

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert - HTTP
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdCurrency =
            await response.Content.ReadFromJsonAsync<CurrencyDto>();

        Assert.NotNull(createdCurrency);
        Assert.True(createdCurrency.CurrencyId > 0);

        Assert.NotNull(response.Headers.Location);
        Assert.Equal(
            $"/api/admin/currencies/{createdCurrency.CurrencyId}",
            response.Headers.Location.AbsolutePath);

        Assert.Equal(request.Code, createdCurrency.Code);
        Assert.Equal(request.Name, createdCurrency.Name);
        Assert.Equal(
            request.ExchangeRateToBase,
            createdCurrency.ExchangeRateToBase);
        Assert.Equal(request.IsActive, createdCurrency.IsActive);

        // Assert - persistence
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var persistedCurrency = await dbContext.Currencies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.CurrencyId == createdCurrency.CurrencyId);

        Assert.NotNull(persistedCurrency);
        Assert.Equal(request.Code, persistedCurrency.Code);
        Assert.Equal(request.Name, persistedCurrency.Name);
        Assert.Equal(
            request.ExchangeRateToBase,
            persistedCurrency.ExchangeRateToBase);
    }

    [Fact]
    public async Task CreateCurrency_NormalizesCodeAndName()
    {
        // Arrange
        var request = CreateValidCurrencyDto() with
        {
            Code = " eur ",
            Name = " Euro "
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdCurrency =
            await response.Content.ReadFromJsonAsync<CurrencyDto>();

        Assert.NotNull(createdCurrency);
        Assert.Equal("EUR", createdCurrency.Code);
        Assert.Equal("Euro", createdCurrency.Name);
    }

    [Fact]
    public async Task CreateCurrency_MissingCode_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidCurrencyDto() with
        {
            Code = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.Equal("Validation failed", problem.Title);
        Assert.Equal(
            CurrencyErrors.CodeRequired.Code,
            problem.Extensions["code"]?.ToString());
    }

    [Fact]
    public async Task CreateCurrency_InvalidCodeLength_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidCurrencyDto() with
        {
            Code = "EURO"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateCurrency_MissingName_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidCurrencyDto() with
        {
            Name = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateCurrency_InvalidExchangeRate_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidCurrencyDto() with
        {
            ExchangeRateToBase = 0
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateCurrency_ExchangeRateWithTooManyDecimals_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidCurrencyDto() with
        {
            ExchangeRateToBase = 4.12345m
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateCurrency_DuplicateCode_ReturnsConflict()
    {
        // Arrange
        var request = CreateValidCurrencyDto();

        var firstResponse = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        // Act
        var secondResponse = await _client.PostAsJsonAsync(
            "/api/admin/currencies",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);

        var problem =
            await secondResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(409, problem.Status);
        Assert.Equal(
            CurrencyErrors.DuplicateCode.Code,
            problem.Extensions["code"]?.ToString());

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var count = await dbContext.Currencies.CountAsync(
            x => x.Code == request.Code);

        Assert.Equal(1, count);
    }

    #endregion


    #region Read Currency Tests

    [Fact]
    public async Task GetCurrencies_ReturnsOkWithCurrencies()
    {
        // Arrange
        await SeedCurrenciesAsync();

        // Act
        var response = await _client.GetAsync(
            "/api/admin/currencies");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var currencies =
            await response.Content.ReadFromJsonAsync<List<CurrencyDto>>();

        Assert.NotNull(currencies);
        Assert.Equal(2, currencies.Count);

        Assert.Contains(
            currencies,
            x => x.Code == "RON");

        Assert.Contains(
            currencies,
            x => x.Code == "EUR");
    }

    [Fact]
    public async Task GetCurrencyById_ExistingCurrency_ReturnsOk()
    {
        // Arrange
        var currencyId = await SeedCurrencyAsync();

        // Act
        var response = await _client.GetAsync(
            $"/api/admin/currencies/{currencyId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var currency =
            await response.Content.ReadFromJsonAsync<CurrencyDto>();

        Assert.NotNull(currency);
        Assert.Equal(currencyId, currency.CurrencyId);
        Assert.Equal("RON", currency.Code);
        Assert.Equal("Romanian Leu", currency.Name);
    }

    [Fact]
    public async Task GetCurrencyById_NonExistingCurrency_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/admin/currencies/{TestConstants.NonExistingId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);

        var problem =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetCurrencyById_InvalidCurrencyId_ReturnsBadRequest(int currencyId)
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/admin/currencies/{currencyId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    #endregion


    #region Update Currency Tests

    [Fact]
    public async Task UpdateCurrency_ValidRequest_ReturnsOkAndPersistsChanges()
    {
        // Arrange
        var currencyId = await SeedCurrencyAsync();

        var request = new UpdateCurrencyDto(
            "RON",
            "Romanian Leu Updated",
            1.1000m,
            false);

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/currencies/{currencyId}",
            request);

        // Assert - HTTP
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedCurrency =
            await response.Content.ReadFromJsonAsync<CurrencyDto>();

        Assert.NotNull(updatedCurrency);
        Assert.Equal(currencyId, updatedCurrency.CurrencyId);
        Assert.Equal("RON", updatedCurrency.Code);
        Assert.Equal(
            "Romanian Leu Updated",
            updatedCurrency.Name);
        Assert.Equal(
            1.1000m,
            updatedCurrency.ExchangeRateToBase);
        Assert.False(updatedCurrency.IsActive);

        // Assert - persistence
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var persistedCurrency = await dbContext.Currencies
            .AsNoTracking()
            .FirstAsync(x => x.CurrencyId == currencyId);

        Assert.Equal(
            "Romanian Leu Updated",
            persistedCurrency.Name);
        Assert.Equal(
            1.1000m,
            persistedCurrency.ExchangeRateToBase);
        Assert.False(persistedCurrency.IsActive);
        Assert.NotNull(persistedCurrency.ModifiedAt);
    }

    [Fact]
    public async Task UpdateCurrency_NonExistingCurrency_ReturnsNotFound()
    {
        // Arrange
        var request = CreateValidUpdateCurrencyDto();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/currencies/{TestConstants.NonExistingId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task UpdateCurrency_InvalidCurrencyId_ReturnsBadRequest(int currencyId)
    {
        // Arrange
        var request = CreateValidUpdateCurrencyDto();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/currencies/{currencyId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateCurrency_DuplicateCode_ReturnsConflict()
    {
        // Arrange
        await SeedCurrenciesAsync();

        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var ron = await dbContext.Currencies
            .AsNoTracking()
            .FirstAsync(x => x.Code == "RON");

        var request = new UpdateCurrencyDto(
            "EUR",
            ron.Name,
            ron.ExchangeRateToBase,
            ron.IsActive);

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/currencies/{ron.CurrencyId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    #endregion


    #region Helpers

    private static CreateCurrencyDto CreateValidCurrencyDto()
    {
        return new CreateCurrencyDto(
            "RON",
            "Romanian Leu",
            1.0000m,
            true);
    }

    private static UpdateCurrencyDto CreateValidUpdateCurrencyDto()
    {
        return new UpdateCurrencyDto(
            "RON",
            "Romanian Leu",
            1.0000m,
            true);
    }

    private async Task<int> SeedCurrencyAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var currency = new Currency
        {
            Code = "RON",
            Name = "Romanian Leu",
            ExchangeRateToBase = 1.0000m,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Currencies.Add(currency);

        await dbContext.SaveChangesAsync();

        return currency.CurrencyId;
    }

    private async Task SeedCurrenciesAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        dbContext.Currencies.AddRange(
            new Currency
            {
                Code = "RON",
                Name = "Romanian Leu",
                ExchangeRateToBase = 1.0000m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Currency
            {
                Code = "EUR",
                Name = "Euro",
                ExchangeRateToBase = 4.9700m,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

        await dbContext.SaveChangesAsync();
    }

    #endregion
}