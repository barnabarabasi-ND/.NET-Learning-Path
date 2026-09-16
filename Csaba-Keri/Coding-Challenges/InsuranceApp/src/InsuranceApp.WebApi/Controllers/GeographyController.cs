using InsuranceApp.Application.Geography;
using InsuranceApp.WebApi.Mappings;
using InsuranceApp.WebApi.Models.Common;
using InsuranceApp.WebApi.Models.Geography;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.WebApi.Controllers;

[ApiController]
[Route("api/brokers")]
public class GeographyController : ControllerBase
{
    private readonly IGeographyService _service;

    public GeographyController(IGeographyService service)
    {
        ArgumentNullException.ThrowIfNull(service);

        _service = service;
    }

    [HttpGet("countries")]
    public async Task<ActionResult<PagedResponse<CountryResponse>>> GetCountries(
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.GetCountriesAsync(request.ToQuery(), cancellationToken);
        
        return Ok(result.ToResponse(country => country.ToResponse()));
    }

    [HttpGet("countries/{countryId}/counties")]
    public async Task<ActionResult<PagedResponse<CountyResponse>>> GetCounties(
        [FromRoute] Guid countryId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.GetCountiesAsync(countryId, request.ToQuery(), cancellationToken);
        
        return Ok(result.ToResponse(county => county.ToResponse()));
    }

    [HttpGet("counties/{countyId}/cities")]
    public async Task<ActionResult<PagedResponse<CityResponse>>> GetCities(
        [FromRoute] Guid countyId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.GetCitiesAsync(countyId, request.ToQuery(), cancellationToken);
        
        return Ok(result.ToResponse(city => city.ToResponse()));
    }
}
