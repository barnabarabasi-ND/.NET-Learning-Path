using InsuranceApp.Application.DTOs.Client;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Infrastructure.Persistence;
using InsuranceApp.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace InsuranceApp.IntegrationTests.Clients;

public sealed class ClientEndpointsTests(
    InsuranceAppWebApplicationFactory factory)
    : IClassFixture<InsuranceAppWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateClient_ValidRequest_ReturnsCreatedAndPersistsClient()
    {
        // Arrange
        var request = CreateValidClient();

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/brokers/clients",
            request);

        // Assert - HTTP response
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdClient =
            await response.Content.ReadFromJsonAsync<ClientDto>();

        Assert.NotNull(createdClient);

        Assert.Equal(request.ClientType, createdClient.ClientType);
        Assert.Equal(request.Name, createdClient.Name);
        Assert.Equal(
            request.IdentificationNumber,
            createdClient.IdentificationNumber);

        // Assert - database persistence
        using var scope = factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<InsuranceDbContext>();

        var persistedClient = await dbContext.Clients.FindAsync(
            createdClient.ClientId);

        Assert.NotNull(persistedClient);
        Assert.Equal(request.Name, persistedClient.Name);
        Assert.Equal(
            request.IdentificationNumber,
            persistedClient.IdentificationNumber);
    }

    [Fact]
    public async Task CreateClient_MissingName_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidClient() with
        {
            Name = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/brokers/clients",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateClient_InvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var request = CreateValidClient() with
        {
            Email = "invalid-email"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/brokers/clients",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateClient_DuplicateIdentificationNumber_ReturnsConflict()
    {
        // Arrange
        var request = CreateValidClient();

        var firstResponse = await _client.PostAsJsonAsync(
            "/api/brokers/clients",
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        // Act - same IdentificationNumber
        var secondResponse = await _client.PostAsJsonAsync(
            "/api/brokers/clients",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);
    }

    private static CreateClientDto CreateValidClient()
    {
        return new CreateClientDto(
            ClientType.Individual,
            "John Doe",
            "1234567890123",
            "john.doe@test.com",
            "0712345678",
            "Cluj-Napoca");
    }

    [Fact]
    public async Task CreateClient_MissingName_ReturnsValidationProblemDetails()
    {
        // Arrange
        var request = CreateValidClient() with
        {
            Name = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/brokers/clients",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var problemDetails =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problemDetails);

        Assert.Equal(
            StatusCodes.Status400BadRequest,
            problemDetails.Status);

        Assert.Equal(
            "Validation failed",
            problemDetails.Title);
    }
}