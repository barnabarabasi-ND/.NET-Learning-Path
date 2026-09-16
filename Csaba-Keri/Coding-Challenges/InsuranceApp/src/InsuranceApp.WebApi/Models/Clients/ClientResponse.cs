namespace InsuranceApp.WebApi.Models.Clients;

public class ClientResponse(
    Guid id,
    ClientTypeDto type,
    string identificationNumber,
    string name,
    string email,
    string phone,
    string? primaryAddress
)
{
    public Guid Id { get; } = id;

    public ClientTypeDto Type { get; } = type;

    public string IdentificationNumber { get; } = identificationNumber;

    public string Name { get; } = name;

    public string Email { get; } = email;

    public string Phone { get; } = phone;

    public string? PrimaryAddress { get; } = primaryAddress;
}
