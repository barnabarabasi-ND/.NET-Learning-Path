namespace InsuranceApp.Application.Clients.Commands;

public record UpdateClientCommand : IClientDetailsCommand
{
    public Guid ClientId { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? PrimaryAddress { get; init; }

    public UpdateClientCommand(
        Guid clientId,
        string? name,
        string? email,
        string? phone,
        string? primaryAddress
    )
    {
        ClientId = clientId;
        Name = name;
        Email = email;
        Phone = phone;
        PrimaryAddress = primaryAddress;
    }
}
