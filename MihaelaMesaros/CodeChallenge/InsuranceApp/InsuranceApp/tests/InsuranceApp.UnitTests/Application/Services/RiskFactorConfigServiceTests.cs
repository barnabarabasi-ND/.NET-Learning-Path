using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.RiskFactorConfig;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Services;
using InsuranceApp.Domain.Entities;
using InsuranceApp.Domain.Enums;
using InsuranceApp.UnitTests.Common;
using Microsoft.Extensions.Logging;
using Moq;

namespace InsuranceApp.UnitTests.Application.Services;

public sealed class RiskFactorConfigServiceTests
{
    private readonly Mock<IRiskFactorConfigRepository> _repositoryMock;
    private readonly Mock<ILogger<RiskFactorConfigService>> _loggerMock;
    private readonly RiskFactorConfigService _service;

    public RiskFactorConfigServiceTests()
    {
        _repositoryMock = new Mock<IRiskFactorConfigRepository>();
        _loggerMock = new Mock<ILogger<RiskFactorConfigService>>();

        _service = new RiskFactorConfigService(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    #region Get All Tests

    [Fact]
    public async Task GetRiskFactorConfigsAsync_ReturnsMappedConfigurations()
    {
        // Arrange
        var configs = new List<RiskFactorConfig>
        {
            CreateRiskFactorConfigEntity(
                level: RiskFactorLevel.Country,
                adjustmentPercentage: 5.25m),

            CreateRiskFactorConfigEntity(
                level: RiskFactorLevel.City,
                adjustmentPercentage: -2.50m)
        };

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigsAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(configs);

        // Act
        var result = await _service.GetRiskFactorConfigsAsync(
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count);

        Assert.Equal(
            configs[0].RiskFactorConfigId,
            result.Value[0].RiskFactorConfigId);

        Assert.Equal(
            configs[1].RiskFactorConfigId,
            result.Value[1].RiskFactorConfigId);
    }

    #endregion

    #region Get By Id Tests

    [Fact]
    public async Task GetRiskFactorConfigByIdAsync_ExistingConfig_ReturnsSuccess()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigByIdAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        // Act
        var result = await _service.GetRiskFactorConfigByIdAsync(
            config.RiskFactorConfigId,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        Assert.Equal(
            config.RiskFactorConfigId,
            result.Value.RiskFactorConfigId);

        Assert.Equal(config.Level, result.Value.Level);
        Assert.Equal(config.ReferenceId, result.Value.ReferenceId);

        Assert.Equal(
            config.AdjustmentPercentage,
            result.Value.AdjustmentPercentage);

        Assert.Equal(config.IsActive, result.Value.IsActive);
    }

    [Fact]
    public async Task GetRiskFactorConfigByIdAsync_EmptyId_ReturnsValidationError()
    {
        // Act
        var result = await _service.GetRiskFactorConfigByIdAsync(
            Guid.Empty,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.Validation,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.InvalidRiskFactorConfigId.Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.GetRiskFactorConfigByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetRiskFactorConfigByIdAsync_NonExistingConfig_ReturnsNotFound()
    {
        // Arrange
        var configId = TestConstants.NonExistingId;

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigByIdAsync(
                configId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((RiskFactorConfig?)null);

        // Act
        var result = await _service.GetRiskFactorConfigByIdAsync(
            configId,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.NotFound,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.NotFound(configId).Code,
            result.Error.Code);
    }

    #endregion

    #region Create Tests

    [Fact]
    public async Task CreateRiskFactorConfigAsync_ValidConfig_ReturnsSuccess()
    {
        // Arrange
        var dto = CreateValidCreateDto();

        SetupExistingReference(dto.Level, dto.ReferenceId);
        SetupNoDuplicate(dto.Level, dto.ReferenceId);

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        Assert.NotEqual(
            Guid.Empty,
            result.Value.RiskFactorConfigId);

        Assert.Equal(dto.Level, result.Value.Level);
        Assert.Equal(dto.ReferenceId, result.Value.ReferenceId);

        Assert.Equal(
            dto.AdjustmentPercentage,
            result.Value.AdjustmentPercentage);

        Assert.Equal(dto.IsActive, result.Value.IsActive);

        _repositoryMock.Verify(
            x => x.AddRiskFactorConfigAsync(
                It.Is<RiskFactorConfig>(config =>
                    config.Level == dto.Level &&
                    config.ReferenceId == dto.ReferenceId &&
                    config.AdjustmentPercentage ==
                        dto.AdjustmentPercentage &&
                    config.IsActive == dto.IsActive),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateRiskFactorConfigAsync_NegativePercentage_ReturnsSuccess()
    {
        // Arrange
        var dto = CreateValidCreateDto() with
        {
            AdjustmentPercentage = -5.25m
        };

        SetupExistingReference(dto.Level, dto.ReferenceId);
        SetupNoDuplicate(dto.Level, dto.ReferenceId);

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(
            -5.25m,
            result.Value!.AdjustmentPercentage);
    }

    [Fact]
    public async Task CreateRiskFactorConfigAsync_InvalidLevel_ReturnsValidationError()
    {
        // Arrange
        var dto = CreateValidCreateDto() with
        {
            Level = (RiskFactorLevel)999
        };

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidLevel);

        VerifyNoDatabaseValidation();
    }

    [Fact]
    public async Task CreateRiskFactorConfigAsync_EmptyReferenceId_ReturnsValidationError()
    {
        // Arrange
        var dto = CreateValidCreateDto() with
        {
            ReferenceId = Guid.Empty
        };

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidReferenceId);

        VerifyNoDatabaseValidation();
    }

    [Theory]
    [InlineData("-100.01")]
    [InlineData("100.01")]
    public async Task CreateRiskFactorConfigAsync_PercentageOutsideRange_ReturnsValidationError(
        string percentage)
    {
        // Arrange
        var dto = CreateValidCreateDto() with
        {
            AdjustmentPercentage = decimal.Parse(
                percentage,
                System.Globalization.CultureInfo.InvariantCulture)
        };

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidAdjustmentPercentage);

        VerifyNoDatabaseValidation();
    }

    [Fact]
    public async Task CreateRiskFactorConfigAsync_PercentageWithTooManyDecimals_ReturnsValidationError()
    {
        // Arrange
        var dto = CreateValidCreateDto() with
        {
            AdjustmentPercentage = 5.123m
        };

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidAdjustmentPercentageScale);

        VerifyNoDatabaseValidation();
    }

    [Fact]
    public async Task CreateRiskFactorConfigAsync_NonExistingReference_ReturnsNotFound()
    {
        // Arrange
        var dto = CreateValidCreateDto();

        _repositoryMock
            .Setup(x => x.ReferenceExistsAsync(
                dto.Level,
                dto.ReferenceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.NotFound,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.ReferenceNotFound.Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.RiskFactorConfigExistsAsync(
                It.IsAny<RiskFactorLevel>(),
                It.IsAny<Guid>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _repositoryMock.Verify(
            x => x.AddRiskFactorConfigAsync(
                It.IsAny<RiskFactorConfig>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateRiskFactorConfigAsync_DuplicateConfig_ReturnsConflict()
    {
        // Arrange
        var dto = CreateValidCreateDto();

        SetupExistingReference(dto.Level, dto.ReferenceId);

        _repositoryMock
            .Setup(x => x.RiskFactorConfigExistsAsync(
                dto.Level,
                dto.ReferenceId,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.Conflict,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.AlreadyExists.Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.AddRiskFactorConfigAsync(
                It.IsAny<RiskFactorConfig>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateRiskFactorConfigAsync_DuplicateOnInsert_ReturnsConflict()
    {
        // Arrange
        var dto = CreateValidCreateDto();

        SetupExistingReference(dto.Level, dto.ReferenceId);
        SetupNoDuplicate(dto.Level, dto.ReferenceId);

        // Simulates race condition:
        // duplicate check passes, but DB unique constraint rejects the INSERT.
        _repositoryMock
            .Setup(x => x.AddRiskFactorConfigAsync(
                It.IsAny<RiskFactorConfig>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new DuplicateEntityException(nameof(RiskFactorConfig)));

        // Act
        var result = await _service.CreateRiskFactorConfigAsync(
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.Conflict,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.AlreadyExists.Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.AddRiskFactorConfigAsync(
                It.IsAny<RiskFactorConfig>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion

    #region Update Tests

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_ValidConfig_ReturnsSuccess()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        var dto = new UpdateRiskFactorConfigDto(
            RiskFactorLevel.City,
            Guid.NewGuid(),
            -3.25m,
            false);

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        SetupExistingReference(dto.Level, dto.ReferenceId);

        _repositoryMock
            .Setup(x => x.RiskFactorConfigExistsAsync(
                dto.Level,
                dto.ReferenceId,
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        Assert.Equal(dto.Level, result.Value.Level);
        Assert.Equal(dto.ReferenceId, result.Value.ReferenceId);

        Assert.Equal(
            dto.AdjustmentPercentage,
            result.Value.AdjustmentPercentage);

        Assert.False(result.Value.IsActive);
        Assert.NotNull(config.ModifiedAt);

        _repositoryMock.Verify(
            x => x.SaveRiskFactorConfigChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_EmptyId_ReturnsValidationError()
    {
        // Arrange
        var dto = CreateValidUpdateDto();

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            Guid.Empty,
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidRiskFactorConfigId);

        _repositoryMock.Verify(
            x => x.GetRiskFactorConfigForUpdateAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_NonExistingConfig_ReturnsNotFound()
    {
        // Arrange
        var configId = TestConstants.NonExistingId;
        var dto = CreateValidUpdateDto();

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                configId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((RiskFactorConfig?)null);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            configId,
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.NotFound,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.NotFound(configId).Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.ReferenceExistsAsync(
                It.IsAny<RiskFactorLevel>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_NonExistingReference_ReturnsNotFound()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();
        var dto = CreateValidUpdateDto();

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        _repositoryMock
            .Setup(x => x.ReferenceExistsAsync(
                dto.Level,
                dto.ReferenceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.NotFound,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.ReferenceNotFound.Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.SaveRiskFactorConfigChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_DuplicateConfig_ReturnsConflict()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();
        var dto = CreateValidUpdateDto();

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        SetupExistingReference(dto.Level, dto.ReferenceId);

        _repositoryMock
            .Setup(x => x.RiskFactorConfigExistsAsync(
                dto.Level,
                dto.ReferenceId,
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.Conflict,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.AlreadyExists.Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.SaveRiskFactorConfigChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_DuplicateCheck_ExcludesCurrentConfig()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        var dto = new UpdateRiskFactorConfigDto(
            config.Level,
            config.ReferenceId,
            7.50m,
            true);

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        SetupExistingReference(dto.Level, dto.ReferenceId);

        _repositoryMock
            .Setup(x => x.RiskFactorConfigExistsAsync(
                dto.Level,
                dto.ReferenceId,
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        _repositoryMock.Verify(
            x => x.RiskFactorConfigExistsAsync(
                dto.Level,
                dto.ReferenceId,
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_DuplicateOnSave_ReturnsConflict()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        var dto = CreateValidUpdateDto();

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        SetupExistingReference(
            dto.Level,
            dto.ReferenceId);

        // Application-level duplicate check passes.
        _repositoryMock
            .Setup(x => x.RiskFactorConfigExistsAsync(
                dto.Level,
                dto.ReferenceId,
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // But DB unique constraint rejects SaveChanges.
        _repositoryMock
            .Setup(x => x.SaveRiskFactorConfigChangesAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new DuplicateEntityException(nameof(RiskFactorConfig)));

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);

        Assert.Equal(
            ErrorType.Conflict,
            result.Error.Type);

        Assert.Equal(
            RiskFactorConfigErrors.AlreadyExists.Code,
            result.Error.Code);

        _repositoryMock.Verify(
            x => x.SaveRiskFactorConfigChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_InvalidLevel_ReturnsValidationError()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        var dto = CreateValidUpdateDto() with
        {
            Level = (RiskFactorLevel)999
        };

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidLevel);

        _repositoryMock.Verify(
            x => x.ReferenceExistsAsync(
                It.IsAny<RiskFactorLevel>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_EmptyReferenceId_ReturnsValidationError()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        var dto = CreateValidUpdateDto() with
        {
            ReferenceId = Guid.Empty
        };

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidReferenceId);

        _repositoryMock.Verify(
            x => x.ReferenceExistsAsync(
                It.IsAny<RiskFactorLevel>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData("-100.01")]
    [InlineData("100.01")]
    public async Task UpdateRiskFactorConfigAsync_PercentageOutsideRange_ReturnsValidationError(
        string percentage)
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        var dto = CreateValidUpdateDto() with
        {
            AdjustmentPercentage = decimal.Parse(
                percentage,
                System.Globalization.CultureInfo.InvariantCulture)
        };

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidAdjustmentPercentage);
    }

    [Fact]
    public async Task UpdateRiskFactorConfigAsync_PercentageWithTooManyDecimals_ReturnsValidationError()
    {
        // Arrange
        var config = CreateRiskFactorConfigEntity();

        var dto = CreateValidUpdateDto() with
        {
            AdjustmentPercentage = 5.123m
        };

        _repositoryMock
            .Setup(x => x.GetRiskFactorConfigForUpdateAsync(
                config.RiskFactorConfigId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(config);

        // Act
        var result = await _service.UpdateRiskFactorConfigAsync(
            config.RiskFactorConfigId,
            dto,
            CancellationToken.None);

        // Assert
        AssertValidationError(
            result,
            RiskFactorConfigErrors.InvalidAdjustmentPercentageScale);
    }
    #endregion

    #region Helpers

    private void SetupExistingReference(
        RiskFactorLevel level,
        Guid referenceId)
    {
        _repositoryMock
            .Setup(x => x.ReferenceExistsAsync(
                level,
                referenceId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    private void SetupNoDuplicate(
        RiskFactorLevel level,
        Guid referenceId)
    {
        _repositoryMock
            .Setup(x => x.RiskFactorConfigExistsAsync(
                level,
                referenceId,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
    }

    private void VerifyNoDatabaseValidation()
    {
        _repositoryMock.Verify(
            x => x.ReferenceExistsAsync(
                It.IsAny<RiskFactorLevel>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _repositoryMock.Verify(
            x => x.RiskFactorConfigExistsAsync(
                It.IsAny<RiskFactorLevel>(),
                It.IsAny<Guid>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static void AssertValidationError(
        Result<RiskFactorConfigDto> result,
        Error expectedError)
    {
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal(expectedError.Code, result.Error.Code);
    }

    private static CreateRiskFactorConfigDto CreateValidCreateDto()
    {
        return new CreateRiskFactorConfigDto(
            RiskFactorLevel.Country,
            Guid.NewGuid(),
            5.25m,
            true);
    }

    private static UpdateRiskFactorConfigDto CreateValidUpdateDto()
    {
        return new UpdateRiskFactorConfigDto(
            RiskFactorLevel.County,
            Guid.NewGuid(),
            -2.50m,
            true);
    }

    private static RiskFactorConfig CreateRiskFactorConfigEntity(
        Guid? riskFactorConfigId = null,
        RiskFactorLevel level = RiskFactorLevel.Country,
        Guid? referenceId = null,
        decimal adjustmentPercentage = 5.25m)
    {
        return new RiskFactorConfig
        {
            RiskFactorConfigId =
                riskFactorConfigId ?? Guid.NewGuid(),

            Level = level,

            ReferenceId =
                referenceId ?? Guid.NewGuid(),

            AdjustmentPercentage =
                adjustmentPercentage,

            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    #endregion
}