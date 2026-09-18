using InsuranceApp.Application.Abstractions.Persistence;
using InsuranceApp.Application.Abstractions.Services;
using InsuranceApp.Application.Common;
using InsuranceApp.Application.DTOs.Geography;

namespace InsuranceApp.Application.Services;

public sealed class GeographyService(IGeographyRepository geographyRepository) : IGeographyService
{
    public async Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken cancellationToken)
    {
        var countries = await geographyRepository.GetCountriesAsync(cancellationToken);

        return countries.Select(x => new CountryDto(x.CountryId, x.Name)).ToList();
    }

    public async Task<Result<IReadOnlyList<CountyDto>>> GetCountiesByCountryAsync(
    int countryId,
    CancellationToken cancellationToken)
    {
        var countryExists = await geographyRepository.CountryExistsAsync(countryId, cancellationToken);

        if (!countryExists)
        {
            return Result<IReadOnlyList<CountyDto>>.Failure(new Error("Geography.CountryNotFound", $"Country with ID {countryId} was not found.", ErrorType.NotFound));
        }

        var counties = await geographyRepository.GetCountiesByCountryAsync(countryId, cancellationToken);

        var countyDtos = counties.Select(x => new CountyDto(x.CountyId, x.Name)) .ToList();

        return Result<IReadOnlyList<CountyDto>>.Success(countyDtos);
    }

    public async Task<Result<IReadOnlyList<CityDto>>> GetCitiesByCountyAsync(int countyId, CancellationToken cancellationToken)
    {
        if (!await geographyRepository.CountyExistsAsync(countyId, cancellationToken))
        {
            return Result<IReadOnlyList<CityDto>>.Failure(new Error("Geography.CountyNotFound", $"County with ID {countyId} was not found.", ErrorType.NotFound));
        }

        var cities = await geographyRepository.GetCitiesByCountyAsync(countyId, cancellationToken);

        var cityDtos = cities.Select(x => new CityDto(x.CityId, x.Name)).ToList();

        return Result<IReadOnlyList<CityDto>>.Success(cityDtos);
    }
}