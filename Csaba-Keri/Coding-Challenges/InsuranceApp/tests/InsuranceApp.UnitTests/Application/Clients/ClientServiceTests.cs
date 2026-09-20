using FluentValidation;
using FluentValidation.Results;
using InsuranceApp.Application.Clients;
using InsuranceApp.Application.Clients.Commands;
using InsuranceApp.Application.Clients.Exceptions;
using InsuranceApp.Application.Clients.Queries;
using InsuranceApp.Application.Common.Exceptions;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Domain.Clients;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace InsuranceApp.UnitTests.Application.Clients;

public sealed class ClientServiceTests
{
    private readonly IClientRepository _repository;
    private readonly IValidator<CreateClientCommand> _createValidator;
    private readonly IValidator<UpdateClientCommand> _updateValidator;
    private readonly IValidator<SearchClientsQuery> _searchValidator;
    private readonly ILogger<ClientService> _logger;

    private readonly ClientService _service;

    public ClientServiceTests()
    {
        _repository = CreateClientRepositoryMock();
        _createValidator = CreateValidatorMock<CreateClientCommand>();
        _updateValidator = CreateValidatorMock<UpdateClientCommand>();
        _searchValidator = CreateValidatorMock<SearchClientsQuery>();
        _logger = CreateLoggerMock<ClientService>();

        _service = new(_repository, _createValidator, _updateValidator, _searchValidator, _logger);
    }

    [Fact]
    public async Task CreateClientAsync_WhenValid_SavesClientAndReturnsResult()
    {
        // Arrange
        var identificationNumber = "1234567890123";
        var command = CreateCommand() with
        {
            IdentificationNumber = $"  {identificationNumber}  "
        };

        // Act
        var result = await _service.CreateClientAsync(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(command.Type, result.Type);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(identificationNumber, result.IdentificationNumber);
        Assert.Equal(command.Email, result.Email);
        Assert.Equal(command.Phone, result.Phone);
        Assert.Equal(command.PrimaryAddress, result.PrimaryAddress);

        await _createValidator.Received(1).ValidateAsync(
            Arg.Is<IValidationContext>(context =>
                ReferenceEquals(context.InstanceToValidate, command) && context.ThrowOnFailures
            ),
            CancellationToken.None
        );

        await _repository.Received(1).ClientExistsByIdentificationNumberAsync(identificationNumber, CancellationToken.None);

        await _repository.Received(1).AddClientAsync(
            Arg.Is<Client>(received =>
                received.Id == result.Id
                && received.Type == command.Type
                && received.Name == command.Name
                && received.IdentificationNumber == identificationNumber
                && received.Email == command.Email
                && received.Phone == command.Phone
                && received.PrimaryAddress == command.PrimaryAddress
            ),
            CancellationToken.None
        );
    }

    [Fact]
    public async Task CreateClientAsync_WhenIdentifierExists_ThrowsWithoutSaving()
    {
        // Arrange
        var command = CreateCommand();

        _repository.ClientExistsByIdentificationNumberAsync(command.IdentificationNumber!, CancellationToken.None)
            .Returns(Task.FromResult(true));

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateClientIdentificationException>(
            () => _service.CreateClientAsync(command, CancellationToken.None)
        );

        await _repository.DidNotReceive().AddClientAsync(Arg.Any<Client>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateClientAsync_WhenValidationFails_ThrowsWithoutRepositoryCalls()
    {
        // Arrange
        var command = CreateCommand();
        var failure = new ValidationException("Rejected by the validator mock.");

        _createValidator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ValidationResult>(failure));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreateClientAsync(command, CancellationToken.None)
        );

        Assert.Same(failure, exception);
        Assert.Empty(_repository.ReceivedCalls());
    }

    [Fact]
    public async Task CreateClientAsync_WhenSaveReportsConcurrentDuplicate_PropagatesConflict()
    {
        // Arrange
        var command = CreateCommand();
        var conflict = new DuplicateClientIdentificationException();

        _repository.AddClientAsync(Arg.Any<Client>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(conflict));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DuplicateClientIdentificationException>(
            () => _service.CreateClientAsync(command, CancellationToken.None)
        );

        Assert.Same(conflict, exception);
    }

    [Fact]
    public async Task GetClientByIdAsync_WhenClientExists_ReturnsClientResult()
    {
        // Arrange
        var client = CreateDomainClient();

        _repository.GetClientByIdAsync(client.Id, CancellationToken.None)
            .Returns(Task.FromResult<Client?>(client)
        );

        // Act
        var result = await _service.GetClientByIdAsync(client.Id, CancellationToken.None);

        // Assert
        Assert.Equal(client.Id, result.Id);
        Assert.Equal(client.Type, result.Type);
        Assert.Equal(client.IdentificationNumber, result.IdentificationNumber);
        Assert.Equal(client.Name, result.Name);
        Assert.Equal(client.Email, result.Email);
        Assert.Equal(client.Phone, result.Phone);
        Assert.Equal(client.PrimaryAddress, result.PrimaryAddress);

        await _repository.Received(1).GetClientByIdAsync(client.Id, CancellationToken.None);
    }

    [Fact]
    public async Task GetClientByIdAsync_WhenClientIsMissing_ThrowsNotFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.GetClientByIdAsync(clientId, CancellationToken.None)
        );

        Assert.Equal(nameof(Client), exception.EntityName);
        Assert.Equal(clientId, exception.EntityId);
    }

    [Fact]
    public async Task GetClientByIdAsync_WhenIdIsEmpty_ThrowsWithoutRepositoryCalls()
    {
        // Arrange
        var clientId = Guid.Empty;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _service.GetClientByIdAsync(clientId, CancellationToken.None)
        );

        Assert.Equal("ClientId", Assert.Single(exception.Errors).PropertyName);
        Assert.Empty(_repository.ReceivedCalls());
    }

    [Fact]
    public async Task UpdateClientAsync_WhenClientExists_SavesDetailsAndPreservesIdentity()
    {
        // Arrange
        var client = CreateDomainClient();
        var command = UpdateCommand(client.Id);

        _repository.GetClientByIdAsync(client.Id, CancellationToken.None)
            .Returns(Task.FromResult<Client?>(client));

        // Act
        var result = await _service.UpdateClientAsync(command, CancellationToken.None);

        // Assert
        Assert.Equal(command.ClientId, result.Id);
        Assert.Equal(client.Type, result.Type);
        Assert.Equal(client.IdentificationNumber, result.IdentificationNumber);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Email, result.Email);
        Assert.Equal(command.Phone, result.Phone);
        Assert.Equal(command.PrimaryAddress, result.PrimaryAddress);

        await _updateValidator.Received(1).ValidateAsync(
            Arg.Is<IValidationContext>(context =>
                ReferenceEquals(context.InstanceToValidate, command) && context.ThrowOnFailures
            ),
            CancellationToken.None
        );

        await _repository.Received(1).UpdateClientAsync(
            Arg.Is<Client>(received =>
                received.Id == command.ClientId
                && received.Type == client.Type
                && received.IdentificationNumber == client.IdentificationNumber
                && received.Name == command.Name
                && received.Email == command.Email
                && received.Phone == command.Phone
                && received.PrimaryAddress == command.PrimaryAddress
            ),
            CancellationToken.None
        );

        await _repository.DidNotReceive().AddClientAsync(Arg.Any<Client>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateClientAsync_WhenClientIsMissing_ThrowsWithoutSaving()
    {
        // Arrange
        var command = UpdateCommand(Guid.NewGuid());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _service.UpdateClientAsync(command, CancellationToken.None)
        );

        Assert.Equal(nameof(Client), exception.EntityName);
        Assert.Equal(command.ClientId, exception.EntityId);

        await _repository.DidNotReceive().UpdateClientAsync(Arg.Any<Client>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateClientAsync_WhenValidationFails_ThrowsWithoutRepositoryCalls()
    {
        // Arrange
        var command = UpdateCommand(Guid.NewGuid());
        var failure = new ValidationException("Rejected by the validator mock.");

        _updateValidator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ValidationResult>(failure));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _service.UpdateClientAsync(command, CancellationToken.None)
        );

        Assert.Same(failure, exception);
        Assert.Empty(_repository.ReceivedCalls());
    }

    [Fact]
    public async Task SearchClientsAsync_WhenValid_NormalizesFiltersAndReturnsMappedPage()
    {
        // Arrange
        var client = CreateDomainClient();
        
        var pageNumber = 2;
        var pageSize = 10;
        var totalCount = 10L;
        var page = new PagedResult<Client>([client], pageNumber, pageSize, totalCount);

        var queryName = "Original";
        var identifier = "";

        var query = new SearchClientsQuery(
            Name: $"  {queryName}  ",
            Identifier: $"  {identifier}  ",
            PageNumber: pageNumber,
            PageSize: pageSize
        );

        _repository.SearchClientsAsync(Arg.Any<SearchClientsQuery>(), CancellationToken.None)
            .Returns(Task.FromResult(page));

        // Act
        var result = await _service.SearchClientsAsync(query, CancellationToken.None);

        // Assert
        var item = Assert.Single(result.Items);
        Assert.Equal(client.Id, item.Id);
        Assert.Equal(client.Name, item.Name);
        Assert.Equal(pageNumber, result.PageNumber);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(totalCount, result.TotalCount);

        await _searchValidator.Received(1).ValidateAsync(
            Arg.Is<IValidationContext>(context =>
                ReferenceEquals(context.InstanceToValidate, query) && context.ThrowOnFailures
            ),
            CancellationToken.None
        );

        await _repository.Received(1).SearchClientsAsync(
            Arg.Is<SearchClientsQuery>(received =>
                received.Name == queryName
                && received.Identifier == null
                && received.PageNumber == pageNumber
                && received.PageSize == pageSize
            ),
            CancellationToken.None
        );
    }

    [Fact]
    public async Task SearchClientsAsync_WhenValidationFails_ThrowsWithoutRepositoryCalls()
    {
        // Arrange
        var query = new SearchClientsQuery();
        var failure = new ValidationException("Rejected by the validator mock.");

        _searchValidator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ValidationResult>(failure));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _service.SearchClientsAsync(query, CancellationToken.None)
        );

        Assert.Same(failure, exception);
        Assert.Empty(_repository.ReceivedCalls());
    }

    private static IClientRepository CreateClientRepositoryMock()
    {
        var repository = Substitute.For<IClientRepository>();

        repository.ClientExistsByIdentificationNumberAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        repository.GetClientByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Client?>(null));

        repository.AddClientAsync(Arg.Any<Client>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.UpdateClientAsync(Arg.Any<Client>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        repository.ClearReceivedCalls();

        return repository;
    }

    private static IValidator<T> CreateValidatorMock<T>()
    {
        var validator = Substitute.For<IValidator<T>>();

        // ValidateAndThrowAsync calls this overload.
        validator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));
        
        validator.ClearReceivedCalls();

        return validator;
    }

    private static ILogger<T> CreateLoggerMock<T>()
    {
        var logger = Substitute.For<ILogger<T>>();

        logger.ClearReceivedCalls();

        return logger;
    }

    private static CreateClientCommand CreateCommand()
    {
        return new(
            Type: ClientType.Individual,
            IdentificationNumber: "1234567890123",
            Name: "Example Client",
            Email: "received@example.com",
            Phone: "+40 700 000 000",
            PrimaryAddress: "Example Street 10"
        );
    }

    private static UpdateClientCommand UpdateCommand(Guid clientId)
    {
        return new(
            ClientId: clientId,
            Name: "Updated Client",
            Email: "updated@example.com",
            Phone: "+40 700 000 001",
            PrimaryAddress: null
        );
    }

    private static Client CreateDomainClient()
    {
        return new(
            id: Guid.NewGuid(),
            type: ClientType.Individual,
            identificationNumber: "1234567890123",
            name: "Original Client",
            email: "original@example.com",
            phone: "+40 700 000 000",
            primaryAddress: "Original Street 10"
        );
    }
}
