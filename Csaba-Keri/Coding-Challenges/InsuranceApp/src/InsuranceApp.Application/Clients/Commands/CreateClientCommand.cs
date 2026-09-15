using InsuranceApp.Domain.Clients;

namespace InsuranceApp.Application.Clients.Commands;

public record CreateClientCommand : IClientDetailsCommand
{
    public ClientType Type { get; init; }
    public string? IdentificationNumber { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? PrimaryAddress { get; init; }

    public CreateClientCommand(
        ClientType type,
        string? identificationNumber,
        string? name,
        string? email,
        string? phone,
        string? primaryAddress = null
    )
    {
        Type = type;
        IdentificationNumber = identificationNumber;
        Name = name;
        Email = email;
        Phone = phone;
        PrimaryAddress = primaryAddress;
    }
}
