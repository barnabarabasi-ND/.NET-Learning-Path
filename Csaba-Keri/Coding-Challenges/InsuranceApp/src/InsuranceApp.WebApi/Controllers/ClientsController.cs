using InsuranceApp.Application.Clients;
using InsuranceApp.WebApi.Mappings;
using InsuranceApp.WebApi.Models.Clients;
using InsuranceApp.WebApi.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.WebApi.Controllers;

[ApiController]
[Route("api/brokers/clients")]
[ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
public class ClientsController : ControllerBase
{
    private readonly IClientService _service;

    public ClientsController(IClientService service)
    {
        ArgumentNullException.ThrowIfNull(service);

        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ClientResponse>>> Search(
        [FromQuery] SearchClientsRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.SearchAsync(request.ToQuery(), cancellationToken);
        
        return Ok(result.ToResponse(client => client.ToResponse()));
    }

    [HttpGet("{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponse>> GetById(
        [FromRoute] Guid clientId,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.GetByIdAsync(clientId, cancellationToken);
        
        return Ok(result.ToResponse());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClientResponse>> Create(
        [FromBody] CreateClientRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.CreateAsync(request.ToCommand(), cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { clientId = result.Id }, result.ToResponse());
    }

    [HttpPut("{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponse>> Update(
        [FromRoute] Guid clientId,
        [FromBody] UpdateClientRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.UpdateAsync(request.ToCommand(clientId), cancellationToken);
        
        return Ok(result.ToResponse());
    }
}
