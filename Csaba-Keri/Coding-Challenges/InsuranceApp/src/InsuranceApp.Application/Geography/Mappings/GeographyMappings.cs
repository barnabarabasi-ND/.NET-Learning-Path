using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Application.Geography.Results;
using InsuranceApp.Domain.Geography;

namespace InsuranceApp.Application.Geography.Mappings;

internal static class GeographyMappings
{
    public static CountryResult ToResult(this Country country)
    {
        return new(
            id: country.Id,
            name: country.Name
        );
    }

    public static PagedResult<CountryResult> ToResult(this PagedResult<Country> page)
    {
        return new(
            items: page.Items.Select(country => country.ToResult()),
            pageNumber: page.PageNumber,
            pageSize: page.PageSize,
            totalCount: page.TotalCount
        );
    }

    public static CountyResult ToResult(this County county)
    {
        return new(
            id: county.Id,
            name: county.Name,
            countryId: county.CountryId
        );
    }

    public static PagedResult<CountyResult> ToResult(this PagedResult<County> page)
    {
        return new(
            items: page.Items.Select(county => county.ToResult()),
            pageNumber: page.PageNumber,
            pageSize: page.PageSize,
            totalCount: page.TotalCount
        );
    }

    public static CityResult ToResult(this City city)
    {
        return new(
            id: city.Id,
            name: city.Name,
            countyId: city.CountyId
        );
    }

    public static PagedResult<CityResult> ToResult(this PagedResult<City> page)
    {
        return new(
            items: page.Items.Select(city => city.ToResult()),
            pageNumber: page.PageNumber,
            pageSize: page.PageSize,
            totalCount: page.TotalCount
        );
    }
}
