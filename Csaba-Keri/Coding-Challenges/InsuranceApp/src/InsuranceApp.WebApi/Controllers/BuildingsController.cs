using InsuranceApp.Application.Buildings;
using InsuranceApp.WebApi.Mappings;
using InsuranceApp.WebApi.Models.Buildings;
using InsuranceApp.WebApi.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.WebApi.Controllers;

[ApiController]
[Route("api/brokers/buildings")]
[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _service;

    public BuildingsController(IBuildingService service)
    {
        ArgumentNullException.ThrowIfNull(service);

        _service = service;
    }

    [HttpGet("/api/brokers/clients/{clientId}/buildings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResponse<BuildingResponse>>> GetByClientId(
        [FromRoute] Guid clientId,
        [FromQuery] PageRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.GetByClientIdAsync(clientId, request.ToQuery(), cancellationToken);
        
        return Ok(result.ToResponse(building => building.ToResponse()));
    }

    [HttpGet("{buildingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BuildingDetailsResponse>> GetById(
        [FromRoute] Guid buildingId,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.GetByIdAsync(buildingId, cancellationToken);
        
        return Ok(result.ToResponse());
    }

    [HttpPost("/api/brokers/clients/{clientId}/buildings")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BuildingDetailsResponse>> Create(
        [FromRoute] Guid clientId,
        [FromBody] SaveBuildingRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.CreateAsync(request.ToCreateCommand(clientId), cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { buildingId = result.Building.Id }, result.ToResponse());
    }

    [HttpPut("{buildingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BuildingDetailsResponse>> Update(
        [FromRoute] Guid buildingId,
        [FromBody] SaveBuildingRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.UpdateAsync(request.ToUpdateCommand(buildingId), cancellationToken);
        
        return Ok(result.ToResponse());
    }
}
