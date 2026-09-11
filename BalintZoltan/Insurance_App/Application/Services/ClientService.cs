namespace Application.Services;

using Application.Abstractions;
using Application.DTO.Clients;
using Domain.Entities;
using Domain.Enums;
using System.Text.RegularExpressions;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<ClientDto> CreateAsync(CreateClientRequest request)
    {
        ValidateIdentificationNumber(request.ClientType, request.IdentificationNumber);

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

        if (client.IdentificationNumber != request.IdentificationNumber)
        {
            throw new InvalidOperationException(
                "The client identification number cannot be changed.");
        }

        client.ChangeType(request.ClientType);
        client.ChangeName(request.Name);
        client.UpdateContactDetails(
            request.Email,
            request.Phone,
            request.Address);

        await _clientRepository.UpdateAsync(client);

        return MapToDto(client);
    }
    private static void ValidateIdentificationNumber(
            ClientType clientType,
            string identificationNumber)
    {
        if (string.IsNullOrWhiteSpace(identificationNumber))
        {
            throw new ArgumentException("Identification number is required.");
        }

        if (clientType == ClientType.Individual &&
            !Regex.IsMatch(identificationNumber, @"^\d{13}$"))
        {
            throw new ArgumentException("CNP must contain exactly 13 digits.");
        }

        if (clientType == ClientType.Company &&
            !Regex.IsMatch(identificationNumber, @"^(RO)?\d{2,10}$",
                RegexOptions.IgnoreCase))
        {
            throw new ArgumentException(
                "CUI must contain 2-10 digits, optionally prefixed with RO.");
        }
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