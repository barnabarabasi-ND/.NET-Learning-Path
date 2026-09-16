namespace InsuranceApp.WebApi.Models.Geography;

public class CountyResponse(Guid id, string name, Guid countryId)
{
    public Guid Id { get; } = id;

    public string Name { get; } = name;

    public Guid CountryId { get; } = countryId;
}
