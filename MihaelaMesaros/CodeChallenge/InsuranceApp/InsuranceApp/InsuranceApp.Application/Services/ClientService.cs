using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Application.Abstractions.Services;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.Client;
using InsuranceApp.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace InsuranceApp.Application.Services;

public sealed class ClientService(IClientRepository clientRepository, ILogger<ClientService> logger) : IClientService
{
    public async Task<Result<PagedResult<ClientDto>>> SearchClientsAsync(ClientSearchDto clientSearchDto, CancellationToken cancellationToken)
    {
        if (clientSearchDto.PageNumber < 1)
        {
            return Result<PagedResult<ClientDto>>.Failure(ClientErrors.InvalidPageNumber);
        }

        if (clientSearchDto.PageSize is < 1 or > 100)
        {
            return Result<PagedResult<ClientDto>>.Failure(ClientErrors.InvalidPageSize);
        }

        var (clients, totalCount) =
            await clientRepository.SearchAsync(
                clientSearchDto.Name?.Trim(),
                clientSearchDto.Identifier?.Trim(),
                clientSearchDto.PageNumber,
                clientSearchDto.PageSize,
                cancellationToken);

        var items = clients.Select(MapToDto).ToList();

        var pagedResult = new PagedResult<ClientDto>(items, clientSearchDto.PageNumber, clientSearchDto.PageSize, totalCount);

        return Result<PagedResult<ClientDto>>.Success(pagedResult);
    }

    public async Task<Result<ClientDto>> GetClientByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        if (clientId <= 0)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidClientId);
        }

        var client = await clientRepository.GetByIdAsync(clientId, cancellationToken);

        if (client is null)
        {
            return Result<ClientDto>.Failure(ClientErrors.NotFound(clientId));
        }

        return Result<ClientDto>.Success(MapToDto(client));
    }

    public async Task<Result<ClientDto>> CreateClientAsync(CreateClientDto createClientDto, CancellationToken cancellationToken)
    {
        var clientType = createClientDto.ClientType;
        var clientName = createClientDto.Name?.Trim();
        var clientEmail = createClientDto.Email?.Trim();
        var clientPhone = createClientDto.Phone?.Trim();
        var clientAddress = createClientDto.Address?.Trim();
        var clientIdentificationNumber = createClientDto.IdentificationNumber?.Trim();

        if (string.IsNullOrWhiteSpace(clientName))
        {
            return Result<ClientDto>.Failure(ClientErrors.NameRequired);
        }

        if (clientName.Length is < 3 or > 200)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidNameLength);
        }

        if (!Enum.IsDefined(clientType))
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidClientType);
        }

        if (!string.IsNullOrWhiteSpace(clientEmail) && (clientEmail.Length > 200 || !MailAddress.TryCreate(clientEmail, out _)))
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidEmail);
        }

        if (!string.IsNullOrWhiteSpace(clientPhone) && clientPhone.Length > 50)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidPhoneLength);
        }

        if (!string.IsNullOrWhiteSpace(clientAddress) && clientAddress.Length > 300)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidAddressLength);
        }


        if (string.IsNullOrWhiteSpace(clientIdentificationNumber))
        {
            return Result<ClientDto>.Failure(ClientErrors.IdentificationNumberRequired);
        }

        if (clientIdentificationNumber.Length is < 3 or > 50)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidIdentificationNumberLength);
        }

        var identificationNumberExists = await clientRepository.IdentificationNumberExistsAsync(clientIdentificationNumber, cancellationToken);

        if (identificationNumberExists)
        {
            return Result<ClientDto>.Failure(ClientErrors.DuplicateIdentificationNumber);
        }

        var client = new Client
        {
            ClientType = clientType,
            Name = clientName,
            IdentificationNumber = clientIdentificationNumber,
            Email = clientEmail,
            Phone = clientPhone,
            Address = clientAddress,
            CreatedAt = DateTime.UtcNow
        };

        await clientRepository.AddAsync(client, cancellationToken);

        await clientRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client {ClientId} created.", client.ClientId);

        return Result<ClientDto>.Success(MapToDto(client));
    }

    public async Task<Result<ClientDto>> UpdateClientAsync(int clientId, UpdateClientDto updateClientDto, CancellationToken cancellationToken)
    {
        if (clientId <= 0)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidClientId);
        }

        var clientName = updateClientDto.Name?.Trim();
        var clientEmail = updateClientDto.Email?.Trim();
        var clientPhone = updateClientDto.Phone?.Trim();
        var clientAddress = updateClientDto.Address?.Trim();

        if (string.IsNullOrWhiteSpace(clientName))
        {
            return Result<ClientDto>.Failure(ClientErrors.NameRequired);
        }

        if (clientName.Length is < 3 or > 200)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidNameLength);
        }

        if (!string.IsNullOrWhiteSpace(clientEmail) && (clientEmail.Length > 200 || !MailAddress.TryCreate(clientEmail, out _)))
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidEmail);
        }

        if (!string.IsNullOrWhiteSpace(clientPhone) && clientPhone.Length > 50)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidPhoneLength);
        }

        if (!string.IsNullOrWhiteSpace(clientAddress) && clientAddress.Length > 300)
        {
            return Result<ClientDto>.Failure(ClientErrors.InvalidAddressLength);
        }


        var client = await clientRepository.GetForUpdateAsync(clientId, cancellationToken);

        if (client is null)
        {
            return Result<ClientDto>.Failure(ClientErrors.NotFound(clientId));
        }

        client.Name = clientName;
        client.Email = clientEmail;
        client.Phone = clientPhone;
        client.Address = clientAddress;
        client.ModifiedAt = DateTime.UtcNow;

        await clientRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client {ClientId} updated.", clientId);

        return Result<ClientDto>.Success(MapToDto(client));
    }


    private static ClientDto MapToDto(Client client)
    {
        return new ClientDto(
            client.ClientId,
            client.ClientType,
            client.Name,
            client.IdentificationNumber,
            client.Email,
            client.Phone,
            client.Address);
    }

}