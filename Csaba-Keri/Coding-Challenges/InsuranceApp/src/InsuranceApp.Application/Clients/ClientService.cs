using FluentValidation;
using FluentValidation.Results;
using InsuranceApp.Application.Clients.Commands;
using InsuranceApp.Application.Clients.Exceptions;
using InsuranceApp.Application.Clients.Mappings;
using InsuranceApp.Application.Clients.Queries;
using InsuranceApp.Application.Clients.Results;
using InsuranceApp.Application.Common.Exceptions;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Domain.Clients;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Clients;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly IValidator<CreateClientCommand> _createValidator;
    private readonly IValidator<UpdateClientCommand> _updateValidator;
    private readonly IValidator<SearchClientsQuery> _searchValidator;
    private readonly ILogger<ClientService> _logger;

    public ClientService(
        IClientRepository repository,
        IValidator<CreateClientCommand> createValidator,
        IValidator<UpdateClientCommand> updateValidator,
        IValidator<SearchClientsQuery> searchValidator,
        ILogger<ClientService> logger
    )
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(createValidator);
        ArgumentNullException.ThrowIfNull(updateValidator);
        ArgumentNullException.ThrowIfNull(searchValidator);
        ArgumentNullException.ThrowIfNull(logger);

        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _searchValidator = searchValidator;
        _logger = logger;
    }

    public async Task<ClientResult> GetByIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (clientId == Guid.Empty)
        {
            throw new ValidationException([
                new ValidationFailure("ClientId", "Client identifier must not be empty.")
            ]);
        }

        var client = await GetClientByIdOrThrowAsync(clientId, cancellationToken);

        return client.ToResult();
    }

    public async Task<ClientResult> CreateAsync(CreateClientCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        cancellationToken.ThrowIfCancellationRequested();
        await _createValidator.ValidateAndThrowAsync(command, cancellationToken);

        var client = new Client(
            id: Guid.NewGuid(),
            type: command.Type,
            identificationNumber: command.IdentificationNumber!,
            name: command.Name!,
            email: command.Email!,
            phone: command.Phone!,
            primaryAddress: command.PrimaryAddress
        );

        var identifierExists = await _repository.ExistsByIdentificationNumberAsync(
            client.IdentificationNumber,
            cancellationToken
        );

        if (identifierExists)
        {
            throw new DuplicateClientIdentificationException();
        }

        await _repository.AddAsync(client, cancellationToken);
        _logger.LogInformation("Client {ClientId} created.", client.Id);

        return client.ToResult();
    }

    public async Task<ClientResult> UpdateAsync(UpdateClientCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        cancellationToken.ThrowIfCancellationRequested();
        await _updateValidator.ValidateAndThrowAsync(command, cancellationToken);

        var client = await GetClientByIdOrThrowAsync(command.ClientId, cancellationToken);

        client.UpdateDetails(
            name: command.Name!,
            email: command.Email!,
            phone: command.Phone!,
            primaryAddress: command.PrimaryAddress
        );

        await _repository.UpdateAsync(client, cancellationToken);
        _logger.LogInformation("Client {ClientId} updated.", client.Id);

        return client.ToResult();
    }

    public async Task<PagedResult<ClientResult>> SearchAsync(SearchClientsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        cancellationToken.ThrowIfCancellationRequested();
        await _searchValidator.ValidateAndThrowAsync(query, cancellationToken);

        var normalizedQuery = query with
        {
            Name = NormalizeOptionalFilter(query.Name),
            Identifier = NormalizeOptionalFilter(query.Identifier)
        };

        var page = await _repository.SearchAsync(normalizedQuery, cancellationToken);

        return page.ToResult();
    }

    private async Task<Client> GetClientByIdOrThrowAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var client = await _repository.GetByIdAsync(clientId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Client), clientId);

        return client;
    }

    private static string? NormalizeOptionalFilter(string? filter)
    {
        return string.IsNullOrWhiteSpace(filter) ? null : filter.Trim();
    }
}
