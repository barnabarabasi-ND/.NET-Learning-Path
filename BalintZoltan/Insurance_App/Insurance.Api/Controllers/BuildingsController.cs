using Application.Abstractions;
using Application.DTO.Buildings;
using Application.DTO.Common;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/brokers")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _buildingService;

    public BuildingsController(IBuildingService buildingService)
    {
        _buildingService = buildingService;
    }

    [HttpGet("buildings/{buildingId:guid}")]
    public async Task<ActionResult<BuildingDto>> GetByIdAsync(Guid buildingId)
    {
        var building = await _buildingService.GetByIdAsync(buildingId);

        if (building is null)
        {
            return NotFound();
        }

        return Ok(building);
    }

    [HttpGet("clients/{clientId:guid}/buildings")]
    public async Task<ActionResult<PagedResult<BuildingDto>>>
        GetByClientIdAsync(
            Guid clientId,
            [FromQuery] PaginationRequest pagination)
    {
        var buildings = await _buildingService.GetByClientIdAsync(
            clientId,
            pagination);

        return Ok(buildings);
    }

    [HttpPost("clients/{clientId:guid}/buildings")]
    public async Task<ActionResult<BuildingDto>> CreateAsync(
        Guid clientId,
        CreateBuildingRequest request)
    {
        request.ClientId = clientId;
        var building = await _buildingService.CreateAsync(request);

        return CreatedAtAction(
            "GetById",
            new { buildingId = building.Id },
            building);
    }

    [HttpPut("buildings/{buildingId:guid}")]
    public async Task<ActionResult<BuildingDto>> UpdateAsync(
        Guid buildingId,
        UpdateBuildingRequest request)
    {
        var building = await _buildingService.UpdateAsync(buildingId, request);

        return Ok(building);
    }
}
