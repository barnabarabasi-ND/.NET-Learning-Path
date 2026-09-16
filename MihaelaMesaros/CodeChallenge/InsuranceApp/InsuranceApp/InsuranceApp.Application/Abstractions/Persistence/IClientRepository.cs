using InsuranceApp.Domain.Entities;

namespace InsuranceApp.Application.Abstractions.Persistence;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(int clientId, CancellationToken cancellationToken);

    Task<bool> IdentificationNumberExistsAsync(string identificationNumber, CancellationToken cancellationToken);

    Task AddAsync(Client client, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}