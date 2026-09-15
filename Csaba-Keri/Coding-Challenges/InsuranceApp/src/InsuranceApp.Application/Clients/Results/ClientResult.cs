using InsuranceApp.Domain.Clients;

namespace InsuranceApp.Application.Clients.Results;

public record ClientResult
{
    public Guid Id { get; }
    public ClientType Type { get; }
    public string IdentificationNumber { get; }
    public string Name { get; }
    public string Email { get; }
    public string Phone { get; }
    public string? PrimaryAddress { get; }

    public ClientResult(
        Guid id,
        ClientType type,
        string identificationNumber,
        string name,
        string email,
        string phone,
        string? primaryAddress
    )
    {
        Id = id;
        Type = type;
        IdentificationNumber = identificationNumber;
        Name = name;
        Email = email;
        Phone = phone;
        PrimaryAddress = primaryAddress;
    }
}
