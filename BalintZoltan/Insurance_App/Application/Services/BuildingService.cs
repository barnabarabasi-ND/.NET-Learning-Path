namespace Application.Services;

using Application.Abstractions;
using Application.DTO.Buildings;
using Domain.Entities;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;

    public BuildingService(IBuildingRepository buildingRepository)
    {
        _buildingRepository = buildingRepository;
    }

    public async Task<BuildingDto> CreateAsync(CreateBuildingRequest request)
    {
        var building = new Building(
            request.ClientId,
            request.CityId,
            request.Street,
            request.Number,
            request.ConstructionYear,
            request.Type,
            request.NumberOfFloors,
            request.SurfaceArea,
            request.InsuredValue,
            request.IsFloodRiskZone,
            request.IsEarthquakeRiskZone);

        await _buildingRepository.AddAsync(building);

        return MapToDto(building);
    }

    public async Task<BuildingDto?> GetByIdAsync(Guid id)
    {
        var building = await _buildingRepository.GetByIdAsync(id);

        if (building is null)
        {
            return null;
        }

        return MapToDto(building);
    }

    public async Task<IReadOnlyCollection<BuildingDto>> GetByClientIdAsync(
        Guid clientId)
    {
        var buildings = await _buildingRepository.GetByClientIdAsync(clientId);

        return buildings
            .Select(MapToDto)
            .ToList();
    }

    public async Task<BuildingDto> UpdateAsync(
        Guid id,
        UpdateBuildingRequest request)
    {
        var building = await _buildingRepository.GetByIdAsync(id);

        if (building is null)
        {
            throw new InvalidOperationException("Building was not found.");
        }

        building.UpdateAddress(
            request.CityId,
            request.Street,
            request.Number);

        building.UpdateDetails(
            request.ConstructionYear,
            request.Type,
            request.NumberOfFloors,
            request.SurfaceArea,
            request.InsuredValue);

        building.UpdateRiskIndicators(
            request.IsFloodRiskZone,
            request.IsEarthquakeRiskZone);

        await _buildingRepository.UpdateAsync(building);

        return MapToDto(building);
    }

    private static BuildingDto MapToDto(Building building)
    {
        return new BuildingDto
        {
            Id = building.Id,
            ClientId = building.ClientId,
            CityId = building.CityId,
            Street = building.Street,
            Number = building.Number,
            ConstructionYear = building.ConstructionYear,
            Type = building.Type,
            NumberOfFloors = building.NumberOfFloors,
            SurfaceArea = building.SurfaceArea,
            InsuredValue = building.InsuredValue,
            IsFloodRiskZone = building.IsFloodRiskZone,
            IsEarthquakeRiskZone = building.IsEarthquakeRiskZone
        };
    }
}