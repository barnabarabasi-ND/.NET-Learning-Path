using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Application.Geography.Results;
using InsuranceApp.Domain.Geography;

namespace InsuranceApp.Application.Geography.Mappings;

internal static class GeographyMappings
{
    public static CountryResult ToResult(this Country country)
    {
        return new(
            Id: country.Id,
            Name: country.Name
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
            Id: county.Id,
            Name: county.Name,
            CountryId: county.CountryId
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
            Id: city.Id,
            Name: city.Name,
            CountyId: city.CountyId
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
