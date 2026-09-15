using InsuranceApp.Application.Clients.Queries;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Domain.Clients;

namespace InsuranceApp.Application.Clients;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid clientId, CancellationToken cancellationToken);

    Task<bool> ExistsByIdentificationNumberAsync(string identificationNumber, CancellationToken cancellationToken);

    Task AddAsync(Client client, CancellationToken cancellationToken);

    Task UpdateAsync(Client client, CancellationToken cancellationToken);

    Task<PagedResult<Client>> SearchAsync(SearchClientsQuery query, CancellationToken cancellationToken);
}
