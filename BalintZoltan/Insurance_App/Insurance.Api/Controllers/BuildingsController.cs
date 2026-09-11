namespace Insurance.Api.Controllers;

using Application.Abstractions;
using Application.DTO.Buildings;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/buildings")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _buildingService;

    public BuildingsController(IBuildingService buildingService)
    {
        _buildingService = buildingService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BuildingDto>> GetById(Guid id)
    {
        var building = await _buildingService.GetByIdAsync(id);

        if (building is null)
        {
            return NotFound();
        }

        return Ok(building);
    }

    [HttpGet("by-client/{clientId:guid}")]
    public async Task<ActionResult<IReadOnlyCollection<BuildingDto>>>
        GetByClientId(Guid clientId)
    {
        var buildings = await _buildingService
            .GetByClientIdAsync(clientId);

        return Ok(buildings);
    }

    [HttpPost]
    public async Task<ActionResult<BuildingDto>> Create(
        CreateBuildingRequest request)
    {
        var building = await _buildingService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = building.Id },
            building);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BuildingDto>> Update(
        Guid id,
        UpdateBuildingRequest request)
    {
        var building = await _buildingService
            .UpdateAsync(id, request);

        return Ok(building);
    }
}