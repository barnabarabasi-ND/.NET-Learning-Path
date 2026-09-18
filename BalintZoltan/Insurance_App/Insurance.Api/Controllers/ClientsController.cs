using Application.Abstractions;
using Application.DTO.Clients;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.Common;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/brokers/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{clientId:guid}")]
    public async Task<ActionResult<ClientDto>> GetClientByIdAsync(Guid clientId)
    {
        var client = await _clientService.GetClientByIdAsync(clientId);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ClientDto>>> SearchClientAsync(
    [FromQuery] string? name,
    [FromQuery] string? identifier,
    [FromQuery] PaginationRequest pagination)
    {
        var clients = await _clientService.SearchClientAsync(
            name,
            identifier,
            pagination);

        return Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateClientAsync(
        CreateClientRequest request)
    {
        var client = await _clientService.CreateClientAsync(request);

        return CreatedAtAction(
            "GetClientById",
            new { clientId = client.Id },
            client);
    }

    [HttpPut("{clientId:guid}")]
    public async Task<ActionResult<ClientDto>> UpdateClientAsync(
        Guid clientId,
        UpdateClientRequest request)
    {
        var client = await _clientService.UpdateClientAsync(clientId, request);

        return Ok(client);
    }
}
