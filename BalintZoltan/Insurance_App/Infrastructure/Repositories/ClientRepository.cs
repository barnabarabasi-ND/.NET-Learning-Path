using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyCollection<Client>> SearchAsync(
        string? searchTerm)
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

        return await query
            .OrderBy(client => client.Name)
            .ToListAsync();
    }

    public async Task UpdateAsync(Client client)
    {
        _dbContext.Clients.Update(client);
        await _dbContext.SaveChangesAsync();
    }
}