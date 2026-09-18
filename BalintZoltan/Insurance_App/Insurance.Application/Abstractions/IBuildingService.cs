using Application.DTO.Buildings;
using Application.DTO.Common;

namespace Application.Abstractions;

public interface IBuildingService
{
    Task<BuildingDto> CreateBuildingAsync(CreateBuildingRequest request);
    Task<BuildingDto?> GetBuildingByIdAsync(Guid id);

    Task<PagedResult<BuildingDto>> GetBuildingByClientIdAsync(
        Guid clientId,
        PaginationRequest pagination);

    Task<BuildingDto> UpdateBuildingAsync(Guid id, UpdateBuildingRequest request);
}
