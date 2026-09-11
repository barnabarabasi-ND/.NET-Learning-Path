namespace Insurance.Api.Controllers;

using Application.Abstractions;
using Application.DTO.Geography;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/geography")]
public class GeographyController : ControllerBase
{
    private readonly IGeographyService _geographyService;

    public GeographyController(IGeographyService geographyService)
    {
        _geographyService = geographyService;
    }

    [HttpGet("countries")]
    public async Task<ActionResult<IReadOnlyCollection<CountryDto>>>
        GetCountries()
    {
        var countries = await _geographyService.GetCountriesAsync();

        return Ok(countries);
    }

    [HttpGet("countries/{countryId:guid}/counties")]
    public async Task<ActionResult<IReadOnlyCollection<CountyDto>>>
        GetCountiesByCountryId(Guid countryId)
    {
        var counties = await _geographyService
            .GetCountiesByCountryIdAsync(countryId);

        return Ok(counties);
    }

    [HttpGet("counties/{countyId:guid}/cities")]
    public async Task<ActionResult<IReadOnlyCollection<CityDto>>>
        GetCitiesByCountyId(Guid countyId)
    {
        var cities = await _geographyService
            .GetCitiesByCountyIdAsync(countyId);

        return Ok(cities);
    }
}