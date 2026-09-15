using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Application.Geography.Results;

namespace InsuranceApp.Application.Geography;

public interface IGeographyService
{
    Task<PagedResult<CountryResult>> GetCountriesAsync(PageQuery query, CancellationToken cancellationToken);

    Task<PagedResult<CountyResult>> GetCountiesAsync(Guid countryId, PageQuery query, CancellationToken cancellationToken);

    Task<PagedResult<CityResult>> GetCitiesAsync(Guid countyId, PageQuery query, CancellationToken cancellationToken);
}
