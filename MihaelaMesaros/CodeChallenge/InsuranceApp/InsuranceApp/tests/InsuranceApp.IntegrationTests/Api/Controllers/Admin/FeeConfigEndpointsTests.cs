using System.Net;
using System.Net.Http.Json;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.FeeConfig;
using InsuranceApp.Domain.Entities;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Infrastructure.Persistence;
using InsuranceApp.IntegrationTests.Common;
using InsuranceApp.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.IntegrationTests.Api.Controllers.Admin;

public sealed class FeeConfigEndpointsTests(
    InsuranceAppWebApplicationFactory factory)
    : IClassFixture<InsuranceAppWebApplicationFactory>, IAsyncLifetime
{
    private readonly InsuranceAppWebApplicationFactory _factory = factory;
    private readonly HttpClient _client = factory.CreateClient();

    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    #region Create Fee Config Tests

    [Fact]
    public async Task CreateFeeConfig_ValidRequest_ReturnsCreatedAndPersistsFeeConfig()
    {
        // Arrange
        var request = CreateValidFeeConfigDto();

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/fees",
            request);

        // Assert - HTTP
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdFee =
            await response.Content.ReadFromJsonAsync<FeeConfigDto>();

        Assert.NotNull(createdFee);
        Assert.NotEqual(Guid.Empty, createdFee.FeeConfigId);

        Assert.NotNull(response.Headers.Location);
        Assert.Equal(
            $"/api/admin/fees/{createdFee.FeeConfigId}",
            response.Headers.Location.AbsolutePath);

        Assert.Equal(request.Name, createdFee.Name);
        Assert.Equal(request.FeeType, createdFee.FeeType);
        Assert.Equal(request.Percentage, createdFee.Percentage);
        Assert.Equal(request.EffectiveFrom, createdFee.EffectiveFrom);
        Assert.Equal(request.EffectiveTo, createdFee.EffectiveTo);
        Assert.Equal(request.IsActive, createdFee.IsActive);

        // Assert - persistence
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var persistedFee = await dbContext.FeeConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.FeeConfigId == createdFee.FeeConfigId);

        Assert.NotNull(persistedFee);
        Assert.Equal(request.Name, persistedFee.Name);
        Assert.Equal(request.FeeType, persistedFee.FeeType);
        Assert.Equal(request.Percentage, persistedFee.Percentage);
    }

    [Fact]
    public async Task CreateFeeConfig_NormalizesName()
    {
        // Arrange
        var request = CreateValidFeeConfigDto() with
        {
            Name = "  Standard broker fee  "
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/fees",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdFee =
            await response.Content.ReadFromJsonAsync<FeeConfigDto>();

        Assert.NotNull(createdFee);
        Assert.Equal("Standard broker fee", createdFee.Name);
    }

    [Fact]
    public async Task CreateFeeConfig_MissingName_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidFeeConfigDto() with
        {
            Name = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/admin/fees",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.Equal(
            FeeConfigErrors.NameRequired.Code,
            problem.Extensions["code"]?.ToString());
    }

    [Fact]
    public async Task CreateFeeConfig_InvalidFeeType_ReturnsBadRequest()
    {
        // FeeType remains an enum, so use an invalid numeric value.
        var request = CreateValidFeeConfigDto() with
        {
            FeeType = (FeeType)999
        };

        var response = await _client.PostAsJsonAsync(
            "/api/admin/fees",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateFeeConfig_InvalidPercentage_ReturnsBadRequest()
    {
        var request = CreateValidFeeConfigDto() with
        {
            Percentage = 101m
        };

        var response = await _client.PostAsJsonAsync(
            "/api/admin/fees",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateFeeConfig_PercentageWithTooManyDecimals_ReturnsBadRequest()
    {
        var request = CreateValidFeeConfigDto() with
        {
            Percentage = 2.12345m
        };

        var response = await _client.PostAsJsonAsync(
            "/api/admin/fees",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateFeeConfig_InvalidEffectivePeriod_ReturnsBadRequest()
    {
        var request = CreateValidFeeConfigDto() with
        {
            EffectiveFrom = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            EffectiveTo = new DateTime(2026, 5, 31, 0, 0, 0, DateTimeKind.Utc)
        };

        var response = await _client.PostAsJsonAsync(
            "/api/admin/fees",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(
            FeeConfigErrors.InvalidEffectivePeriod.Code,
            problem.Extensions["code"]?.ToString());
    }

    #endregion

    #region Read Fee Config Tests

    [Fact]
    public async Task GetFees_ReturnsOkWithFeeConfigs()
    {
        // Arrange
        await SeedFeeConfigsAsync();

        // Act
        var response = await _client.GetAsync(
            "/api/admin/fees");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fees =
            await response.Content.ReadFromJsonAsync<List<FeeConfigDto>>();

        Assert.NotNull(fees);
        Assert.Equal(2, fees.Count);

        Assert.Contains(
            fees,
            x => x.Name == "Standard broker fee");

        Assert.Contains(
            fees,
            x => x.Name == "Admin fee");
    }

    [Fact]
    public async Task GetFeeById_ExistingFeeConfig_ReturnsOk()
    {
        // Arrange
        var feeConfigId = await SeedFeeConfigAsync();

        // Act
        var response = await _client.GetAsync(
            $"/api/admin/fees/{feeConfigId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var fee =
            await response.Content.ReadFromJsonAsync<FeeConfigDto>();

        Assert.NotNull(fee);
        Assert.Equal(feeConfigId, fee.FeeConfigId);
        Assert.Equal("Standard broker fee", fee.Name);
        Assert.Equal(FeeType.BrokerCommission, fee.FeeType);
    }

    [Fact]
    public async Task GetFeeById_NonExistingFeeConfig_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/admin/fees/{TestConstants.NonExistingId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetFeeById_EmptyFeeConfigId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync(
            $"/api/admin/fees/{Guid.Empty}");

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    #endregion

    #region Update Fee Config Tests

    [Fact]
    public async Task UpdateFeeConfig_ValidRequest_ReturnsOkAndPersistsChanges()
    {
        // Arrange
        var feeConfigId = await SeedFeeConfigAsync();

        var request = new UpdateFeeConfigDto(
            "Updated broker fee",
            FeeType.BrokerCommission,
            5.5000m,
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
            false);

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/fees/{feeConfigId}",
            request);

        // Assert - HTTP
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedFee =
            await response.Content.ReadFromJsonAsync<FeeConfigDto>();

        Assert.NotNull(updatedFee);
        Assert.Equal(feeConfigId, updatedFee.FeeConfigId);
        Assert.Equal("Updated broker fee", updatedFee.Name);
        Assert.Equal(5.5000m, updatedFee.Percentage);
        Assert.False(updatedFee.IsActive);

        // Assert - persistence
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var persistedFee = await dbContext.FeeConfigs
            .AsNoTracking()
            .FirstAsync(x => x.FeeConfigId == feeConfigId);

        Assert.Equal("Updated broker fee", persistedFee.Name);
        Assert.Equal(5.5000m, persistedFee.Percentage);
        Assert.False(persistedFee.IsActive);
        Assert.NotNull(persistedFee.ModifiedAt);
    }

    [Fact]
    public async Task UpdateFeeConfig_NonExistingFeeConfig_ReturnsNotFound()
    {
        // Arrange
        var request = CreateValidUpdateFeeConfigDto();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/fees/{TestConstants.NonExistingId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateFeeConfig_EmptyFeeConfigId_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidUpdateFeeConfigDto();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/fees/{Guid.Empty}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateFeeConfig_InvalidEffectivePeriod_ReturnsBadRequest()
    {
        // Arrange
        var feeConfigId = await SeedFeeConfigAsync();

        var request = CreateValidUpdateFeeConfigDto() with
        {
            EffectiveFrom = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            EffectiveTo = new DateTime(2026, 5, 31, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/admin/fees/{feeConfigId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    #endregion

    #region Helpers

    private static CreateFeeConfigDto CreateValidFeeConfigDto()
    {
        return new CreateFeeConfigDto(
            "Standard broker fee",
            FeeType.BrokerCommission,
            2.5000m,
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
            true);
    }

    private static UpdateFeeConfigDto CreateValidUpdateFeeConfigDto()
    {
        return new UpdateFeeConfigDto(
            "Standard broker fee",
            FeeType.BrokerCommission,
            2.5000m,
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
            true);
    }

    private async Task<Guid> SeedFeeConfigAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var fee = new FeeConfig
        {
            FeeConfigId = Guid.NewGuid(),
            Name = "Standard broker fee",
            FeeType = FeeType.BrokerCommission,
            Percentage = 2.5000m,
            EffectiveFrom = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EffectiveTo = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.FeeConfigs.Add(fee);

        await dbContext.SaveChangesAsync();

        return fee.FeeConfigId;
    }

    private async Task SeedFeeConfigsAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        dbContext.FeeConfigs.AddRange(
            new FeeConfig
            {
                FeeConfigId = Guid.NewGuid(),
                Name = "Standard broker fee",
                FeeType = FeeType.BrokerCommission,
                Percentage = 2.5000m,
                EffectiveFrom = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EffectiveTo = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new FeeConfig
            {
                FeeConfigId = Guid.NewGuid(),
                Name = "Admin fee",
                FeeType = FeeType.AdminFee,
                Percentage = 1.0000m,
                EffectiveFrom = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EffectiveTo = null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

        await dbContext.SaveChangesAsync();
    }

    #endregion
}
