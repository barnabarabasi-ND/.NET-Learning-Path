namespace Application.Services;

using Application.Abstractions;
using Application.DTOs.Clients;
using Domain.Entities;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<ClientDto> CreateAsync(CreateClientRequest request)
    {
        var exists = await _clientRepository
            .ExistsByIdentificationNumberAsync(request.IdentificationNumber);

        if (exists)
        {
            throw new InvalidOperationException(
                "A client with this identification number already exists.");
        }

        var client = new Client(
            request.ClientType,
            request.Name,
            request.IdentificationNumber,
            request.Email,
            request.Phone,
            request.Address);

        await _clientRepository.AddAsync(client);

        return new ClientDto
        {
            Id = client.Id,
            ClientType = client.Type,
            Name = client.Name,
            IdentificationNumber = client.IdentificationNumber,
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address
        };
    }
    public async Task<ClientDto?> GetByIdAsync(Guid id)
    {
        var client = await _clientRepository.GetByIdAsync(id);

        if (client is null)
        {
            return null;
        }

        return MapToDto(client);
    }
    public async Task<IReadOnlyCollection<ClientDto>> SearchAsync(
        string? searchTerm)
    {
        var clients = await _clientRepository.SearchAsync(searchTerm);

        return clients
            .Select(MapToDto)
            .ToList();
    }

    public async Task<ClientDto> UpdateAsync(
           Guid id,
           UpdateClientRequest request)
    {
        var client = await _clientRepository.GetByIdAsync(id);

        if (client is null)
        {
            throw new InvalidOperationException("Client was not found.");
        }

        var exists = await _clientRepository
            .ExistsByIdentificationNumberAsync(
                request.IdentificationNumber,
                id);

        if (exists)
        {
            throw new InvalidOperationException(
                "A client with this identification number already exists.");
        }

        client.ChangeType(request.ClientType);
        client.ChangeName(request.Name);
        client.ChangeIdentificationNumber(request.IdentificationNumber);
        client.UpdateContactDetails(
            request.Email,
            request.Phone,
            request.Address);

        await _clientRepository.UpdateAsync(client);

        return MapToDto(client);
    }

    private static ClientDto MapToDto(Client client)
    {
        return new ClientDto
        {
            Id = client.Id,
            ClientType = client.Type,
            Name = client.Name,
            IdentificationNumber = client.IdentificationNumber,
            Email = client.Email,
            Phone = client.Phone,
            Address = client.Address
        };
    }
}