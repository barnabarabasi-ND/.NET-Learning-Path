using Application.DTO.Buildings;
using Application.DTO.Common;

namespace Application.Abstractions;

public interface IBuildingService
{
    Task<BuildingDto> CreateAsync(CreateBuildingRequest request);
    Task<BuildingDto?> GetByIdAsync(Guid id);

    Task<PagedResult<BuildingDto>> GetByClientIdAsync(
        Guid clientId,
        PaginationRequest pagination);

    Task<BuildingDto> UpdateAsync(Guid id, UpdateBuildingRequest request);
}
