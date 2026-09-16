using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories;

internal sealed class ClientRepository(InsuranceDbContext dbContext) : IClientRepository
{
    public async Task<(IReadOnlyList<Client> Items, int TotalCount)> SearchAsync(string? name, string? identificationNumber, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.Clients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(x => x.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(identificationNumber))
        {
            query = query.Where(x => x.IdentificationNumber == identificationNumber);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var clients = await query
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (clients, totalCount);
    }

    public Task<Client?> GetByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        return dbContext.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId, cancellationToken);
    }

    public Task<bool> IdentificationNumberExistsAsync(string identificationNumber, CancellationToken cancellationToken)
    {
        return dbContext.Clients.AnyAsync(x => x.IdentificationNumber == identificationNumber, cancellationToken);
    }

    public async Task AddAsync(Client client, CancellationToken cancellationToken)
    {
        await dbContext.Clients.AddAsync(client, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Client?> GetForUpdateAsync(int clientId, CancellationToken cancellationToken)
    {
        return dbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId, cancellationToken);
    }
}