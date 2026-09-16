namespace InsuranceApp.WebApi.Models.Buildings;

public class NamedLocationResponse(Guid id, string name)
{
    public Guid Id { get; } = id;

    public string Name { get; } = name;
}
