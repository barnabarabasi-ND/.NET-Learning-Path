using Domain.Entities;

namespace Application.Abstractions;

public interface IBuildingRepository
{
    Task<Building?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Building>> GetByClientIdAsync(Guid clientId);
    Task AddAsync(Building building);
    Task UpdateAsync(Building building);
}