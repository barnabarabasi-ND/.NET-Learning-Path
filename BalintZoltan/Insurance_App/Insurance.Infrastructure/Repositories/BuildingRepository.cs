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

    public async Task AddBuildingAsync(Building building, CancellationToken cancellationToken = default)
    {
        await _dbContext.Buildings.AddAsync(building, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Building?> GetBuildingByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Buildings
            .AsNoTracking()
            .FirstOrDefaultAsync(building => building.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Building>> GetBuildingByClientIdAsync(
        Guid clientId,
        PaginationRequest pagination,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Buildings
            .AsNoTracking()
            .Where(building => building.ClientId == clientId);

        var totalCount = await query.CountAsync(cancellationToken);

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
            .ToListAsync(cancellationToken);

        return new PagedResult<Building>
        {
            Items = buildings,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task UpdateBuildingAsync(Building building, CancellationToken cancellationToken = default)
    {
        _dbContext.Buildings.Update(building);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
