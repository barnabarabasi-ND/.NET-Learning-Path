using Application.DTO.Clients;
using Application.DTO.Common;

namespace Application.Abstractions;

public interface IClientService
{
    Task<ClientDto> CreateClientAsync(CreateClientRequest request);

    Task<ClientDto?> GetClientByIdAsync(Guid id);

    Task<PagedResult<ClientDto>> SearchClientAsync(
        string? name,
        string? identifier,
        PaginationRequest pagination);

    Task<ClientDto> UpdateClientAsync(Guid id, UpdateClientRequest request);
}