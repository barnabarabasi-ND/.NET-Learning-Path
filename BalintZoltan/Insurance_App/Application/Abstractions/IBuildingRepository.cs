namespace Application.Abstractions;

using Domain.Entities;

public interface IBuildingRepository
{
    Task<Building?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Building>> GetByClientIdAsync(Guid clientId);
    Task AddAsync(Building building);
    Task UpdateAsync(Building building);
}