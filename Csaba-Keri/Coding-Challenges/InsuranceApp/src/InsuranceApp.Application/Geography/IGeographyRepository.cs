using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Domain.Geography;

namespace InsuranceApp.Application.Geography;

public interface IGeographyRepository
{
    Task<bool> CountryExistsAsync(Guid countryId, CancellationToken cancellationToken);

    Task<bool> CountyExistsAsync(Guid countyId, CancellationToken cancellationToken);

    Task<PagedResult<Country>> GetCountriesAsync(PageQuery query, CancellationToken cancellationToken);

    Task<PagedResult<County>> GetCountiesAsync(Guid countryId, PageQuery query, CancellationToken cancellationToken);

    Task<PagedResult<City>> GetCitiesAsync(Guid countyId, PageQuery query, CancellationToken cancellationToken);
}
