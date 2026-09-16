namespace InsuranceApp.WebApi.Models.Geography;

public class CountryResponse(Guid id, string name)
{
    public Guid Id { get; } = id;

    public string Name { get; } = name;
}
