using InsuranceApp.Domain.Brokers;

namespace InsuranceApp.Application.Brokers.Results;

public record BrokerResult(
    Guid Id,
    string Code,
    string Name,
    string Email,
    string Phone,
    BrokerStatus Status
)
{
    public Guid Id { get; } = Id;
    public string Code { get; } = Code;
    public string Name { get; } = Name;
    public string Email { get; } = Email;
    public string Phone { get; } = Phone;
    public BrokerStatus Status { get; } = Status;
}
