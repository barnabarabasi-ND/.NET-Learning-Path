using Application.Abstractions;
using Application.DTO.Common;
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

    public async Task<PagedResult<Building>> GetByClientIdAsync(
        Guid clientId,
        PaginationRequest pagination)
    {
        var query = _dbContext.Buildings
            .AsNoTracking()
            .Where(building => building.ClientId == clientId);

        var totalCount = await query.CountAsync();

        var pageNumber = Math.Max(pagination.PageNumber, 1);
        var pageSize = Math.Min(
            Math.Max(pagination.PageSize, 1),
            100);

        var buildings = await query
            .OrderBy(building => building.Street)
            .ThenBy(building => building.Number)
            .ThenBy(building => building.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Building>
        {
            Items = buildings,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task UpdateAsync(Building building)
    {
        _dbContext.Buildings.Update(building);
        await _dbContext.SaveChangesAsync();
    }
}
