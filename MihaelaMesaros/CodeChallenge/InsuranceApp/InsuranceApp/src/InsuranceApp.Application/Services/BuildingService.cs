using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Application.Abstractions.Services;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.Building;
using InsuranceApp.Application.Models.Building;
using InsuranceApp.Domain.Constants;
using InsuranceApp.Domain.Entities;
using InsuranceApp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Services;

public sealed class BuildingService(
    IBuildingRepository buildingRepository, 
    IClientRepository clientRepository,
    IGeographyRepository geographyRepository,
    ILogger<BuildingService> logger
) : IBuildingService
{
    public async Task<Result<BuildingDto>> GetBuildingByIdAsync(int buildingId, CancellationToken cancellationToken)
    {
        if (buildingId <= 0)
        {
            return Result<BuildingDto>.Failure(BuildingErrors.InvalidBuildingId);
        }

        var building = await buildingRepository.GetBuildingByIdAsync(buildingId, cancellationToken);

        if (building is null)
        {
            return Result<BuildingDto>.Failure(BuildingErrors.NotFound(buildingId));
        }

        return Result<BuildingDto>.Success(MapBuildingToDto(building));
    }

    public async Task<Result<IReadOnlyList<BuildingDto>>> GetBuildingsByClientAsync(int clientId, CancellationToken cancellationToken)
    {
        if (clientId <= 0)
        {
            return Result<IReadOnlyList<BuildingDto>>.Failure(BuildingErrors.InvalidClientId);
        }

        var client = await clientRepository.GetClientByIdAsync(clientId, cancellationToken);

        if (client is null)
        {
            return Result<IReadOnlyList<BuildingDto>>.Failure(ClientErrors.NotFound(clientId));
        }

        var buildings = await buildingRepository.GetBuildingsByClientAsync(clientId, cancellationToken);

        var buildingDtos = buildings.Select(MapBuildingToDto).ToList();

        return Result<IReadOnlyList<BuildingDto>>.Success(buildingDtos);
    }

    public async Task<Result<BuildingDto>> CreateBuildingForClientAsync(int clientId, CreateBuildingDto createBuildingDto, CancellationToken cancellationToken)
    {
        var cityId = createBuildingDto.CityId;
        var addressStreet = createBuildingDto.AddressStreet?.Trim();
        var addressStreetNumber = createBuildingDto.AddressStreetNumber?.Trim();
        var constructionYear = createBuildingDto.ConstructionYear;
        var buildingType = createBuildingDto.BuildingType;
        var numberOfFloors = createBuildingDto.NumberOfFloors;
        var surfaceArea = createBuildingDto.SurfaceArea;
        var insuredValue = createBuildingDto.InsuredValue;
        var riskIndicators = createBuildingDto.RiskIndicators?.Trim();

        if (clientId <= 0)
        {
            return Result<BuildingDto>.Failure(BuildingErrors.InvalidClientId);
        }

        var client = await clientRepository.GetClientByIdAsync(clientId, cancellationToken);

        if (client is null)
        {
            return Result<BuildingDto>.Failure(ClientErrors.NotFound(clientId));
        }

        var validationBuildingDetails = ValidateBuildingDetails(
            new BuildingDetails(
                cityId,
                addressStreet,
                addressStreetNumber,
                constructionYear,
                buildingType,
                numberOfFloors,
                surfaceArea,
                insuredValue,
                riskIndicators
            )
        );

        if (validationBuildingDetails is not null)
        {
            return Result<BuildingDto>.Failure(validationBuildingDetails);
        }

        var validationBuildingCity = await ValidateCityAsync(cityId, cancellationToken);

        if (validationBuildingCity is not null)
        {
            return Result<BuildingDto>.Failure(validationBuildingCity);
        }


        var building = new Building
        {
            ClientId = clientId,
            CityId = cityId,
            AddressStreet = addressStreet!,
            AddressStreetNumber = addressStreetNumber!,
            ConstructionYear = constructionYear,
            BuildingType = buildingType,
            NumberOfFloors = numberOfFloors,
            SurfaceArea = surfaceArea,
            InsuredValue = insuredValue,
            RiskIndicators = riskIndicators,
            CreatedAt = DateTime.UtcNow
        };

        await buildingRepository.AddBuildingAsync(building, cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Building {BuildingId} created.", building.BuildingId);
        }

        return Result<BuildingDto>.Success(MapBuildingToDto(building));
    }

    public async Task<Result<BuildingDto>> UpdateBuildingAsync(int buildingId, UpdateBuildingDto updateBuildingDto, CancellationToken cancellationToken)
    {
        if (buildingId <= 0)
        {
            return Result<BuildingDto>.Failure(BuildingErrors.InvalidBuildingId);
        }

        var cityId = updateBuildingDto.CityId;
        var addressStreet = updateBuildingDto.AddressStreet?.Trim();
        var addressStreetNumber = updateBuildingDto.AddressStreetNumber?.Trim();
        var constructionYear = updateBuildingDto.ConstructionYear;
        var buildingType = updateBuildingDto.BuildingType;
        var numberOfFloors = updateBuildingDto.NumberOfFloors;
        var surfaceArea = updateBuildingDto.SurfaceArea;
        var insuredValue = updateBuildingDto.InsuredValue;
        var riskIndicators = updateBuildingDto.RiskIndicators?.Trim();

        var validationBuildingDetails = ValidateBuildingDetails(
            new BuildingDetails(
                cityId,
                addressStreet,
                addressStreetNumber,
                constructionYear,
                buildingType,
                numberOfFloors,
                surfaceArea,
                insuredValue,
                riskIndicators
            )
        );

        if (validationBuildingDetails is not null)
        {
            return Result<BuildingDto>.Failure(validationBuildingDetails);
        }

        var validationBuildingCity = await ValidateCityAsync(cityId, cancellationToken);

        if (validationBuildingCity is not null)
        {
            return Result<BuildingDto>.Failure(validationBuildingCity);
        }


        var building = await buildingRepository.GetBuildingForUpdateAsync(buildingId, cancellationToken);

        if (building is null)
        {
            return Result<BuildingDto>.Failure(BuildingErrors.NotFound(buildingId));
        }

        building.CityId = cityId;
        building.AddressStreet = addressStreet!;
        building.AddressStreetNumber = addressStreetNumber!;
        building.ConstructionYear = constructionYear;
        building.BuildingType = buildingType;
        building.NumberOfFloors = numberOfFloors;
        building.SurfaceArea = surfaceArea;
        building.InsuredValue = insuredValue;
        building.RiskIndicators = riskIndicators;
        building.ModifiedAt = DateTime.UtcNow;

        await buildingRepository.SaveBuildingChangesAsync(cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Building {BuildingId} updated.", buildingId);
        }

        return Result<BuildingDto>.Success(MapBuildingToDto(building));
    }

    private static BuildingDto MapBuildingToDto(Building building)
    {
        return new BuildingDto(
            building.BuildingId,
            building.ClientId,
            building.AddressStreet,
            building.AddressStreetNumber,
            building.CityId,
            building.ConstructionYear,
            building.BuildingType,
            building.NumberOfFloors,
            building.SurfaceArea,
            building.InsuredValue,
            building.RiskIndicators);
    }

    private async Task<Error?> ValidateCityAsync(int cityId, CancellationToken cancellationToken)
    {
        if (cityId <= 0)
        {
            return BuildingErrors.InvalidCityId;
        }

        var cityExists = await geographyRepository.CityExistsAsync(cityId, cancellationToken);

        return cityExists
            ? null
            : BuildingErrors.CityNotFound(cityId);
    }

    private static Error? ValidateBuildingDetails(BuildingDetails buildingDetails)
    {
        if (string.IsNullOrWhiteSpace(buildingDetails.AddressStreet))
        {
            return BuildingErrors.AddressStreetRequired;
        }

        if (buildingDetails.AddressStreet.Length > BuildingConstraints.AddressStreetMaxLength)
        {
            return BuildingErrors.InvalidAddressStreetLength;
        }

        if (string.IsNullOrWhiteSpace(buildingDetails.AddressStreetNumber))
        {
            return BuildingErrors.AddressStreetNumberRequired;
        }

        if (buildingDetails.AddressStreetNumber.Length > BuildingConstraints.AddressStreetNumberMaxLength)
        {
            return BuildingErrors.InvalidAddressStreetNumberLength;
        }

        if (buildingDetails.ConstructionYear < BuildingConstraints.MinConstructionYear 
            || buildingDetails.ConstructionYear > DateTime.UtcNow.Year)
        {
            return BuildingErrors.InvalidConstructionYear;
        }

        if (!Enum.IsDefined(buildingDetails.BuildingType))
        {
            return BuildingErrors.InvalidBuildingType;
        }

        if (buildingDetails.NumberOfFloors < BuildingConstraints.MinNumberOfFloors
            || buildingDetails.NumberOfFloors > BuildingConstraints.MaxNumberOfFloors)
        {
            return BuildingErrors.InvalidNumberOfFloors;
        }

        if (!DecimalValidation.HasValidScale(buildingDetails.SurfaceArea, CommonConstraints.DecimalScale))
        {
            return BuildingErrors.InvalidSurfaceAreaScale;
        }

        if (buildingDetails.SurfaceArea < BuildingConstraints.MinSurfaceArea 
            || buildingDetails.SurfaceArea > BuildingConstraints.MaxSurfaceArea)
        {
            return BuildingErrors.InvalidSurfaceArea;
        }

        if (!DecimalValidation.HasValidScale(buildingDetails.InsuredValue, CommonConstraints.DecimalScale))
        {
            return BuildingErrors.InvalidInsuredValueScale;
        }

        if (buildingDetails.InsuredValue < BuildingConstraints.MinInsuredValue 
            || buildingDetails.InsuredValue > BuildingConstraints.MaxInsuredValue)
        {
            return BuildingErrors.InvalidInsuredValue;
        }

        if (!string.IsNullOrWhiteSpace(buildingDetails.RiskIndicators) 
            && buildingDetails.RiskIndicators.Length > BuildingConstraints.RiskIndicatorsMaxLength)
        {
            return BuildingErrors.InvalidRiskIndicatorsLength;
        }

        return null;
    }
}