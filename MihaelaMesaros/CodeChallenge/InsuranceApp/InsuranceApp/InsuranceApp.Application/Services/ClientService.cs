using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Application.Abstractions.Services;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.Client;
using InsuranceApp.Domain.Entities;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Services;

public sealed class ClientService(IClientRepository clientRepository, ILogger<ClientService> logger) : IClientService
{
    public async Task<Result<ClientDto>> GetClientByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(clientId, cancellationToken);

        if (client is null)
        {
            return Result<ClientDto>.Failure(ClientErrors.NotFound(clientId));
        }

        var clientDto = new ClientDto(
            client.ClientId,
            client.ClientType,
            client.Name,
            client.IdentificationNumber,
            client.Email,
            client.Phone,
            client.Address);

        return Result<ClientDto>.Success(clientDto);
    }

    public async Task<Result<ClientDto>> CreateClientAsync(CreateClientDto createClientDto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(createClientDto.Name))
        {
            return Result<ClientDto>.Failure(ClientErrors.NameRequired);
        }

        if (string.IsNullOrWhiteSpace(createClientDto.IdentificationNumber))
        {
            return Result<ClientDto>.Failure(ClientErrors.IdentificationNumberRequired);
        }

        if (!Enum.IsDefined(createClientDto.ClientType))
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidClientType);
        }

        if (!string.IsNullOrWhiteSpace(createClientDto.Email) && !MailAddress.TryCreate(createClientDto.Email, out _))
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidEmail);
        }

        var identificationNumber = createClientDto.IdentificationNumber.Trim();

        var identificationNumberExists = await clientRepository.IdentificationNumberExistsAsync(identificationNumber, cancellationToken);

        if (identificationNumberExists)
        {
            return Result<ClientDto>.Failure(ClientErrors.DuplicateIdentificationNumber);
        }

        var client = new Client
        {
            ClientType = createClientDto.ClientType,
            Name = createClientDto.Name.Trim(),
            IdentificationNumber = identificationNumber,
            Email = createClientDto.Email?.Trim(),
            Phone = createClientDto.Phone?.Trim(),
            Address = createClientDto.Address?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await clientRepository.AddAsync(client, cancellationToken);

        await clientRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client {ClientId} created.", client.ClientId);

        var clientDto = new ClientDto(
            client.ClientId,
            client.ClientType,
            client.Name,
            client.IdentificationNumber,
            client.Email,
            client.Phone,
            client.Address);

        return Result<ClientDto>.Success(clientDto);
    }
}