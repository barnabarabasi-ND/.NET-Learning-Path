using Application.DTO.Common;
using Domain.Entities;

namespace Application.Abstractions;

public interface IBuildingRepository
{
    Task<Building?> GetBuildingByIdAsync(Guid id);

    Task<PagedResult<Building>> GetBuildingByClientIdAsync(
        Guid clientId,
        PaginationRequest pagination);

    Task AddBuildingAsync(Building building);
    Task UpdateBuildingAsync(Building building);
}
