using InsuranceApp.Domain.Entities;

namespace InsuranceApp.Application.Abstractions.Persistence;

public interface IClientRepository
{
    Task<(IReadOnlyList<Client> Items, int TotalCount)> SearchAsync(string? name, string? identificationNumber, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<Client?> GetByIdAsync(int clientId, CancellationToken cancellationToken);

    Task<bool> IdentificationNumberExistsAsync(string identificationNumber, CancellationToken cancellationToken);

    Task AddAsync(Client client, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Client?> GetForUpdateAsync(int clientId, CancellationToken cancellationToken);
}