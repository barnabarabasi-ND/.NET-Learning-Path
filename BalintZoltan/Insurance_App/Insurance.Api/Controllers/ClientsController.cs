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
    public async Task<ActionResult<ClientDto>> GetById(Guid clientId)
    {
        var client = await _clientService.GetByIdAsync(clientId);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpGet]

    public async Task<ActionResult<PagedResult<ClientDto>>> Search(
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
    public async Task<ActionResult<ClientDto>> Create(
        CreateClientRequest request)
    {
        var client = await _clientService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { clientId = client.Id },
            client);
    }

    [HttpPut("{clientId:guid}")]
    public async Task<ActionResult<ClientDto>> Update(
        Guid clientId,
        UpdateClientRequest request)
    {
        var client = await _clientService.UpdateAsync(clientId, request);

        return Ok(client);
    }
}
