using Application.Abstractions;
using Application.DTO.Buildings;
using Application.DTO.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
    public async Task<ActionResult<BuildingDto>> GetBuildingByIdAsync(Guid buildingId)
    {
        var building = await _buildingService.GetBuildingByIdAsync(buildingId);

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
        var buildings = await _buildingService.GetBuildingByClientIdAsync(
            clientId,
            pagination);

        return Ok(buildings);
    }

    [HttpPost("clients/{clientId:guid}/buildings")]
    public async Task<ActionResult<BuildingDto>> CreateBuildingAsync(
        Guid clientId,
        CreateBuildingRequest request)
    {
        request.ClientId = clientId;
        var building = await _buildingService.CreateBuildingAsync(request);

        return CreatedAtAction(
            "GetBuildingById",
            new { buildingId = building.Id },
            building);
    }

    [HttpPut("buildings/{buildingId:guid}")]
    public async Task<ActionResult<BuildingDto>> UpdateBuildingAsync(
        Guid buildingId,
        UpdateBuildingRequest request)
    {
        var building = await _buildingService.UpdateBuildingAsync(buildingId, request);

        return Ok(building);
    }
}
