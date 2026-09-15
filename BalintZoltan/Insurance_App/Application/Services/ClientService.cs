using Application.Abstractions;
using Application.DTO.Clients;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    private async Task CheckClientIdentificationNumberExistAsync(string identificationNumber)
    {
        var exists = await _clientRepository
            .ExistsByIdentificationNumberAsync(identificationNumber);

        if (exists)
        {
            throw new InvalidOperationException(
                "A client with this identification number already exists.");
        }


    }
    public async Task<ClientDto> CreateAsync(CreateClientRequest request)
    {
        ValidateIdentificationNumber(request.ClientType, request.IdentificationNumber);
        ValidateEmail(request.Email);
        await CheckClientIdentificationNumberExistAsync(request.IdentificationNumber);

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

        if (client.IdentificationNumber != request.IdentificationNumber)
        {
            throw new InvalidOperationException(
                "The client identification number cannot be changed.");
        }

        if (request.Email != null)
        {
            ValidateEmail(request.Email);
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

    private static void ValidateEmail(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            return;
        }

        var email = Email.Trim();

        if (email.Length > 254)
        {
            throw new ArgumentException(
                "Email cannot be longer than 254 characters.");
        }

        var emailAttribute = new EmailAddressAttribute();

        if (!emailAttribute.IsValid(email))
        {
            throw new ArgumentException(
                "Invalid email address format.");
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