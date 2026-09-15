using FluentValidation;
using FluentValidation.Results;
using InsuranceApp.Application.Common.Exceptions;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Application.Geography;
using InsuranceApp.Domain.Geography;
using NSubstitute;

namespace InsuranceApp.UnitTests.Application.Geography;

public sealed class GeographyServiceTests
{
    private readonly IGeographyRepository _repository;
    private readonly IValidator<PageQuery> _pageValidator;

    private readonly GeographyService _service;

    public GeographyServiceTests()
    {
        _repository = CreateGeographyRepositoryMock();
        _pageValidator = CreateValidatorMock<PageQuery>();

        _service = new(_repository, _pageValidator);
    }

    [Fact]
    public async Task GetCountriesAsync_WhenValid_ReturnsMappedPageAndForwardsToken()
    {
        // Arrange
        using var cancellationSource = new CancellationTokenSource();

        var country = new Country(Guid.NewGuid(), "Romania");

        var pageNumber = 2;
        var pageSize = 1;
        var query = new PageQuery(pageNumber, pageSize);

        var totalCount = 2L;
        var page = new PagedResult<Country>([country], pageNumber, pageSize, totalCount);

        _repository.GetCountriesAsync(query, cancellationSource.Token)
            .Returns(Task.FromResult(page));

        // Act
        var result = await _service.GetCountriesAsync(query, cancellationSource.Token);

        // Assert
        var item = Assert.Single(result.Items);
        Assert.Equal(country.Id, item.Id);
        Assert.Equal(country.Name, item.Name);
        Assert.Equal(pageNumber, result.PageNumber);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(totalCount, result.TotalCount);

        await _pageValidator.Received(1).ValidateAsync(
            Arg.Is<IValidationContext>(context =>
                ReferenceEquals(context.InstanceToValidate, query) && context.ThrowOnFailures
            ),
            cancellationSource.Token
        );

        await _repository.Received(1).GetCountriesAsync(query, cancellationSource.Token);
    }

    [Fact]
    public async Task GetCountiesAsync_WhenCountryExists_ReturnsCountiesForThatCountry()
    {
        // Arrange
        var countryId = Guid.NewGuid();
        var county = new County(Guid.NewGuid(), "Cluj", countryId);
        var query = new PageQuery();

        var pageNumber = 1;
        var pageSize = 20;
        var totalCount = 1L;
        var page = new PagedResult<County>([county], pageNumber, pageSize, totalCount);

        _repository.CountryExistsAsync(countryId, CancellationToken.None)
            .Returns(Task.FromResult(true));

        _repository.GetCountiesAsync(countryId, query, CancellationToken.None)
            .Returns(Task.FromResult(page));

        // Act
        var result = await _service.GetCountiesAsync(countryId, query, CancellationToken.None);

        // Assert
        var item = Assert.Single(result.Items);
        Assert.Equal(county.Id, item.Id);
        Assert.Equal(county.Name, item.Name);
        Assert.Equal(countryId, item.CountryId);
        Assert.Equal(pageNumber, result.PageNumber);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(totalCount, result.TotalCount);

        await _pageValidator.Received(1).ValidateAsync(
            Arg.Is<IValidationContext>(context =>
                ReferenceEquals(context.InstanceToValidate, query) && context.ThrowOnFailures
            ),
            CancellationToken.None
        );

        await _repository.Received(1).CountryExistsAsync(countryId, CancellationToken.None);
        await _repository.Received(1).GetCountiesAsync(countryId, query, CancellationToken.None);
    }

    [Fact]
    public async Task GetCitiesAsync_WhenCountyExists_ReturnsCitiesForThatCounty()
    {
        // Arrange
        var countyId = Guid.NewGuid();
        var city = new City(Guid.NewGuid(), "Cluj-Napoca", countyId);
        var query = new PageQuery();

        var pageNumber = 1;
        var pageSize = 20;
        var totalCount = 1L;
        var page = new PagedResult<City>([city], pageNumber, pageSize, totalCount);

        _repository.CountyExistsAsync(countyId, CancellationToken.None)
            .Returns(Task.FromResult(true));
        
        _repository.GetCitiesAsync(countyId, query, CancellationToken.None)
            .Returns(Task.FromResult(page));

        // Act
        var result = await _service.GetCitiesAsync(countyId, query, CancellationToken.None);

        // Assert
        var item = Assert.Single(result.Items);
        Assert.Equal(city.Id, item.Id);
        Assert.Equal(city.Name, item.Name);
        Assert.Equal(countyId, item.CountyId);
        Assert.Equal(pageNumber, result.PageNumber);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(totalCount, result.TotalCount);

        await _pageValidator.Received(1).ValidateAsync(
            Arg.Is<IValidationContext>(context =>
                ReferenceEquals(context.InstanceToValidate, query) && context.ThrowOnFailures
            ),
            CancellationToken.None
        );

        await _repository.Received(1).CountyExistsAsync(countyId, CancellationToken.None);
        await _repository.Received(1).GetCitiesAsync(countyId, query, CancellationToken.None);
    }

    [Theory]
    [InlineData(true, nameof(Country))]
    [InlineData(false, nameof(County))]
    public async Task ChildQueries_WhenParentIsMissing_ThrowWithoutListingChildren(
        bool queryCounties, string entityName
    )
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var query = new PageQuery();

        Func<Task> action = queryCounties
            ? () => _service.GetCountiesAsync(parentId, query, CancellationToken.None)
            : () => _service.GetCitiesAsync(parentId, query, CancellationToken.None);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(action);

        Assert.Equal(entityName, exception.EntityName);
        Assert.Equal(parentId, exception.EntityId);
        
        await _repository.DidNotReceive().GetCountiesAsync(
            Arg.Any<Guid>(), Arg.Any<PageQuery>(), Arg.Any<CancellationToken>()
        );
        
        await _repository.DidNotReceive().GetCitiesAsync(
            Arg.Any<Guid>(), Arg.Any<PageQuery>(), Arg.Any<CancellationToken>()
        );
    }

    [Theory]
    [InlineData(true, "CountryId")]
    [InlineData(false, "CountyId")]
    public async Task ChildQueries_WhenParentIdIsEmpty_ThrowWithoutRepositoryCalls(
        bool queryCounties, string propertyName
    )
    {
        // Arrange
        var query = new PageQuery();

        Func<Task> action = queryCounties
            ? () => _service.GetCountiesAsync(Guid.Empty, query, CancellationToken.None)
            : () => _service.GetCitiesAsync(Guid.Empty, query, CancellationToken.None);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(action);

        Assert.Equal(propertyName, Assert.Single(exception.Errors).PropertyName);
        Assert.Empty(_repository.ReceivedCalls());
    }

    [Fact]
    public async Task GetCountriesAsync_WhenValidationFails_ThrowsWithoutRepositoryCalls()
    {
        // Arrange
        var query = new PageQuery();
        var failure = new ValidationException("Rejected by the validator mock.");

        _pageValidator.ValidateAsync(Arg.Any<IValidationContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<ValidationResult>(failure));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _service.GetCountriesAsync(query, CancellationToken.None)
        );

        Assert.Same(failure, exception);
        Assert.Empty(_repository.ReceivedCalls());
    }

    [Fact]
    public async Task GetCitiesAsync_WhenRequestedPageIsEmpty_PreservesTotalCount()
    {
        // Arrange
        var countyId = Guid.NewGuid();
        var pageNumber = 3;
        var pageSize = 10;
        var totalCount = 14L;

        var query = new PageQuery(pageNumber, pageSize);
        var page = new PagedResult<City>([], pageNumber, pageSize, totalCount);

        _repository.CountyExistsAsync(countyId, CancellationToken.None)
            .Returns(Task.FromResult(true));

        _repository.GetCitiesAsync(countyId, query, CancellationToken.None)
            .Returns(Task.FromResult(page));

        // Act
        var result = await _service.GetCitiesAsync(countyId, query, CancellationToken.None);

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(pageNumber, result.PageNumber);
        Assert.Equal(pageSize, result.PageSize);
        Assert.Equal(totalCount, result.TotalCount);

        await _repository.Received(1).CountyExistsAsync(countyId, CancellationToken.None);
    }

    private static IGeographyRepository CreateGeographyRepositoryMock()
    {
        var repository = Substitute.For<IGeographyRepository>();

        repository.CountryExistsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        repository.CountyExistsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));
        
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
}
