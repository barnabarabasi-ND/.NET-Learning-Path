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
    public async Task<ActionResult<ClientDto>> GetByIdAsync(Guid clientId)
    {
        var client = await _clientService.GetByIdAsync(clientId);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpGet]

    public async Task<ActionResult<PagedResult<ClientDto>>> SearchAsync(
    [FromQuery] string? name,
    [FromQuery] string? identifier,
    [FromQuery] PaginationRequest pagination)
    {
        var clients = await _clientService.SearchAsync(
            name,
            identifier,
            pagination);

        return Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateAsync(
        CreateClientRequest request)
    {
        var client = await _clientService.CreateAsync(request);

        return CreatedAtAction(
            "GetById",
            new { clientId = client.Id },
            client);
    }

    [HttpPut("{clientId:guid}")]
    public async Task<ActionResult<ClientDto>> UpdateAsync(
        Guid clientId,
        UpdateClientRequest request)
    {
        var client = await _clientService.UpdateAsync(clientId, request);

        return Ok(client);
    }
}
