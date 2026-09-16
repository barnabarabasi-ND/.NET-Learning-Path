using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApp.Infrastructure.Persistence.Repositories;

internal sealed class ClientRepository(InsuranceDbContext dbContext) : IClientRepository
{
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
}