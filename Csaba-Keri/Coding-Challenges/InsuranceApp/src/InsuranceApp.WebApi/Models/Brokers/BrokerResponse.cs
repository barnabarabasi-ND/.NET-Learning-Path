namespace InsuranceApp.WebApi.Models.Brokers;

public record BrokerResponse(
    Guid Id,
    string Code,
    string Name,
    string Email,
    string Phone,
    BrokerStatusDto Status
)
{
    public Guid Id { get; } = Id;

    public string Code { get; } = Code;

    public string Name { get; } = Name;

    public string Email { get; } = Email;

    public string Phone { get; } = Phone;

    public BrokerStatusDto Status { get; } = Status;
}
