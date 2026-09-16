using Application.Abstractions;
using Application.DTO.Common;
using Domain.Entities;

namespace Application.UnitTests.Fakes;

public sealed class FakeClientRepository : IClientRepository
{
    public readonly Dictionary<Guid, Client> Storage = new();

    public void Seed(Client client) => Storage[client.Id] = client;

    public Task AddAsync(Client client)
    {
        Storage[client.Id] = client;
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByIdentificationNumberAsync(
        string identificationNumber,
        Guid? excludedClientId = null)
    {
        var exists = Storage.Values.Any(client =>
            client.IdentificationNumber == identificationNumber
            && client.Id != excludedClientId);

        return Task.FromResult(exists);
    }

    public Task<Client?> GetByIdAsync(Guid id)
    {
        Storage.TryGetValue(id, out var client);
        return Task.FromResult(client);
    }

    public Task<PagedResult<Client>> SearchAsync(
        string? name,
        string? identifier,
        PaginationRequest pagination)
    {
        var query = Storage.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(client =>
                client.Name.Contains(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(identifier))
        {
            query = query.Where(client =>
                client.IdentificationNumber == identifier.Trim());
        }

        var all = query
            .OrderBy(client => client.Name)
            .ThenBy(client => client.Id)
            .ToList();

        var pageNumber = Math.Max(pagination.PageNumber, 1);
        var pageSize = Math.Min(Math.Max(pagination.PageSize, 1), 100);

        return Task.FromResult(new PagedResult<Client>
        {
            Items = all.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = all.Count
        });
    }

    public Task UpdateAsync(Client client)
    {
        Storage[client.Id] = client;
        return Task.CompletedTask;
    }
}
