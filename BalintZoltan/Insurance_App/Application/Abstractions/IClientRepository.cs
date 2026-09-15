using Application.DTO.Common;
using Domain.Entities;


namespace Application.Abstractions;

public interface IClientRepository
{
    Task AddAsync(Client client);
    Task<bool> ExistsByIdentificationNumberAsync(
        string identificationNumber,
        Guid? excludedClientId = null);

    Task<Client?> GetByIdAsync(Guid id);

    Task<PagedResult<Client>> SearchAsync(
        string? searchTerm,
        PaginationRequest pagination);

    Task UpdateAsync(Client client);
}


