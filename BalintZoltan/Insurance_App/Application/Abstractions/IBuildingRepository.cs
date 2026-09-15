using Application.DTO.Common;
using Domain.Entities;

namespace Application.Abstractions;

public interface IBuildingRepository
{
    Task<Building?> GetByIdAsync(Guid id);

    Task<PagedResult<Building>> GetByClientIdAsync(
        Guid clientId,
        PaginationRequest pagination);

    Task AddAsync(Building building);
    Task UpdateAsync(Building building);
}
