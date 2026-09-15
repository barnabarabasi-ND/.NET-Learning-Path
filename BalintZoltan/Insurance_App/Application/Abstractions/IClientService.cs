using Application.DTO.Clients;
using Application.DTO.Common;

namespace Application.Abstractions;

public interface IClientService
{
    Task<ClientDto> CreateAsync(CreateClientRequest request);

    Task<ClientDto?> GetByIdAsync(Guid id);

    Task<PagedResult<ClientDto>> SearchAsync(
        string? searchTerm,
        PaginationRequest pagination);

    Task<ClientDto> UpdateAsync(Guid id, UpdateClientRequest request);
}