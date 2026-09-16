using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.Client;
using InsuranceApp.Application.Services;
using InsuranceApp.Domain.Entities;
using InsuranceApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace InsuranceApp.UnitTests.Services;

public sealed class ClientServiceTests
{
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly Mock<ILogger<ClientService>> _loggerMock;
    private readonly ClientService _service;

    public ClientServiceTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _loggerMock = new Mock<ILogger<ClientService>>();

        _service = new ClientService(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task CreateClientAsync_ValidClient_ReturnsSuccess()
    {
        // Arrange
        var dto = CreateValidClient();

        _repositoryMock
            .Setup(x => x.IdentificationNumberExistsAsync(
                dto.IdentificationNumber,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateClientAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        Assert.Equal(dto.Name, result.Value.Name);
        Assert.Equal(
            dto.IdentificationNumber,
            result.Value.IdentificationNumber);

        // Verify that the correct Client was sent to the repository
        _repositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Client>(client =>
                    client.Name == dto.Name &&
                    client.IdentificationNumber == dto.IdentificationNumber &&
                    client.ClientType == dto.ClientType),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateClientAsync_MissingName_ReturnsValidationError()
    {
        // Arrange
        var dto = CreateValidClient() with
        {
            Name = ""
        };

        // Act
        var result = await _service.CreateClientAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(ErrorType.Validation, result.Error.Type);

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateClientAsync_MissingIdentificationNumber_ReturnsValidationError()
    {
        // Arrange
        var dto = CreateValidClient() with
        {
            IdentificationNumber = ""
        };

        // Act
        var result = await _service.CreateClientAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(ErrorType.Validation, result.Error.Type);

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateClientAsync_DuplicateIdentificationNumber_ReturnsConflict()
    {
        // Arrange
        var dto = CreateValidClient();

        _repositoryMock
            .Setup(x => x.IdentificationNumberExistsAsync(
                dto.IdentificationNumber,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateClientAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateClientAsync_InvalidEmail_ReturnsValidationError()
    {
        // Arrange
        var dto = CreateValidClient() with
        {
            Email = "invalid-email"
        };

        // Act
        var result = await _service.CreateClientAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(ErrorType.Validation, result.Error.Type);

        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
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
}