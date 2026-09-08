namespace Application.Abstractions;

using Domain.Entities;

public interface IClientRepository
{
    Task AddAsync(Client client);
    Task<bool> ExistsByIdentificationNumberAsync(
        string identificationNumber,
        Guid? excludedClientId = null);

    Task<Client?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<Client>> SearchAsync(string? searchTerm);

    Task UpdateAsync(Client client);
}


