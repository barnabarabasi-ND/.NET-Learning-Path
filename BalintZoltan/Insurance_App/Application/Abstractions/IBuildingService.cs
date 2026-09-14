using Application.DTO.Buildings;

namespace Application.Abstractions;

public interface IBuildingService
{
    Task<BuildingDto> CreateAsync(CreateBuildingRequest request);
    Task<BuildingDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<BuildingDto>> GetByClientIdAsync(Guid clientId);
    Task<BuildingDto> UpdateAsync(Guid id, UpdateBuildingRequest request);
}