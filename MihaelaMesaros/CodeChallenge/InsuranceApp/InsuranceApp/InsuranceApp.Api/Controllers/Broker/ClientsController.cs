using InsuranceApp.Api.Common;
using InsuranceApp.Application.Abstractions.Services;
using InsuranceApp.Application.DTOs.Client;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.Api.Controllers.Broker;

/// <summary>
/// Controller for managing clients in the broker context.
/// </summary>
/// <param name="clientService">The client service.</param>
[ApiController]
[Route("api/brokers/clients")]
public sealed class ClientsController(IClientService clientService) : ControllerBase
{
    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="request">The client creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created client.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClientDto>> CreateClientAsync(CreateClientDto request, CancellationToken cancellationToken)
    {
        var result = await clientService.CreateClientAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error!.ToProblemResult();
        }

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    /// <summary>
    /// Gets a client by identifier.
    /// </summary>
    [HttpGet("{clientId:int}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientDto>> GetClientByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        var result = await clientService.GetClientByIdAsync(clientId, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error!.ToProblemResult();
        }

        return Ok(result.Value);
    }
}