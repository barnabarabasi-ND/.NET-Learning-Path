using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Common;

namespace Infrastructure.Repositories;

public sealed class ClientRepository : IClientRepository
{
    private readonly InsuranceDbContext _dbContext;

    public ClientRepository(InsuranceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Client client)
    {
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByIdentificationNumberAsync(
        string identificationNumber,
        Guid? excludedClientId = null)
    {
        return await _dbContext.Clients.AnyAsync(client =>
            client.IdentificationNumber == identificationNumber
            && (!excludedClientId.HasValue
                || client.Id != excludedClientId.Value));
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Clients
            .FirstOrDefaultAsync(client => client.Id == id);
    }

    public async Task<PagedResult<Client>> SearchAsync(
        string? searchTerm,
        PaginationRequest pagination)
    {
        var query = _dbContext.Clients
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var normalizedSearchTerm = searchTerm.Trim();

            query = query.Where(client =>
                client.Name.Contains(normalizedSearchTerm)
                || client.IdentificationNumber == normalizedSearchTerm);
        }

        var totalCount = await query.CountAsync();

        var pageNumber = Math.Max(pagination.PageNumber, 1);
        var pageSize = Math.Min(
            Math.Max(pagination.PageSize, 1),
            100);

        var clients = await query
            .OrderBy(client => client.Name)
            .ThenBy(client => client.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Client>
        {
            Items = clients,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task UpdateAsync(Client client)
    {
        _dbContext.Clients.Update(client);
        await _dbContext.SaveChangesAsync();
    }
}