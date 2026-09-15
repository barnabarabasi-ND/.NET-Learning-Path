using InsuranceApp.Application.Clients.Commands;
using InsuranceApp.Application.Clients.Queries;
using InsuranceApp.Application.Clients.Results;
using InsuranceApp.Application.Common.Pagination;

namespace InsuranceApp.Application.Clients;

public interface IClientService
{
    Task<ClientResult> GetByIdAsync(Guid clientId, CancellationToken cancellationToken);

    Task<ClientResult> CreateAsync(CreateClientCommand command, CancellationToken cancellationToken);

    Task<ClientResult> UpdateAsync(UpdateClientCommand command, CancellationToken cancellationToken);

    Task<PagedResult<ClientResult>> SearchAsync(SearchClientsQuery query, CancellationToken cancellationToken);
}
