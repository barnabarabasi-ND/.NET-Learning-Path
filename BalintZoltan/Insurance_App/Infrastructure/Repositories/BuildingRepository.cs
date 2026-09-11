using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class BuildingRepository : IBuildingRepository
{
    private readonly InsuranceDbContext _dbContext;

    public BuildingRepository(InsuranceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Building building)
    {
        await _dbContext.Buildings.AddAsync(building);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Building?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Buildings
            .AsNoTracking()
            .FirstOrDefaultAsync(building => building.Id == id);
    }

    public async Task<IReadOnlyCollection<Building>> GetByClientIdAsync(
        Guid clientId)
    {
        return await _dbContext.Buildings
            .AsNoTracking()
            .Where(building => building.ClientId == clientId)
            .OrderBy(building => building.Street)
            .ThenBy(building => building.Number)
            .ToListAsync();
    }

    public async Task UpdateAsync(Building building)
    {
        _dbContext.Buildings.Update(building);
        await _dbContext.SaveChangesAsync();
    }
}