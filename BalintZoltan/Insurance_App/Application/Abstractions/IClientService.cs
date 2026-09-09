namespace Application.Abstractions;

using Application.DTO.Clients;

public interface IClientService
{
    Task<ClientDto> CreateAsync(CreateClientRequest request);

    Task<ClientDto?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<ClientDto>> SearchAsync(string? searchTerm);

    Task<ClientDto> UpdateAsync(Guid id, UpdateClientRequest request);
}