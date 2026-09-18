using Application.DTO.Clients;
using Application.DTO.Common;
using Domain.Entities;


namespace Application.Abstractions;

public interface IClientRepository
{
    Task AddClientAsync(Client client);
    Task<bool> ExistsClientByIdentificationNumberAsync(
        string identificationNumber,
        Guid? excludedClientId = null);

    Task<Client?> GetClientByIdAsync(Guid id);

    Task<PagedResult<Client>> SearchClientAsync(
        string? name,
        string? identifier,
        PaginationRequest pagination);

    Task UpdateClientAsync(Client client);
}


