namespace Insurance.Api.Controllers;

using Application.Abstractions;
using Application.DTO.Clients;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClientDto>> GetById(Guid id)
    {
        var client = await _clientService.GetByIdAsync(id);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ClientDto>>> Search(
        [FromQuery] string? searchTerm)
    {
        var clients = await _clientService.SearchAsync(searchTerm);

        return Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(
        CreateClientRequest request)
    {
        var client = await _clientService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = client.Id },
            client);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ClientDto>> Update(
        Guid id,
        UpdateClientRequest request)
    {
        var client = await _clientService.UpdateAsync(id, request);

        return Ok(client);
    }
}