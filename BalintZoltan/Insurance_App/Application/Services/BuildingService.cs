using Application.Abstractions;
using Application.DTO.Buildings;
using Domain.Entities;

namespace Application.Services;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IGeographyRepository _geographyRepository;

    private async Task CheckClientExistAsync(Guid clientId)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);

        if (client is null)
        {
            throw new InvalidOperationException("Client was not found.");
        }
    }

    private async Task CheckCityExistAsync(Guid cityId)
    {
        var cityExists = await _geographyRepository.CityExistsAsync(cityId);

        if (!cityExists)
        {
            throw new InvalidOperationException("City was not found.");
        }
    }


    public BuildingService(
        IBuildingRepository buildingRepository,
        IClientRepository clientRepository,
        IGeographyRepository geographyRepository)
    {
        _buildingRepository = buildingRepository;
        _clientRepository = clientRepository;
        _geographyRepository = geographyRepository;
    }

    public async Task<BuildingDto> CreateAsync(CreateBuildingRequest request)
    {
        await CheckClientExistAsync(request.ClientId);
        await CheckCityExistAsync(request.CityId);

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

        await CheckCityExistAsync(request.CityId);

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