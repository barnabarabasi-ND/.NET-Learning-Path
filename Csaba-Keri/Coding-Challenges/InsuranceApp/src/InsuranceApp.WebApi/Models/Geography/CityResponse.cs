namespace InsuranceApp.WebApi.Models.Geography;

public class CityResponse(Guid id, string name, Guid countyId)
{
    public Guid Id { get; } = id;

    public string Name { get; } = name;

    public Guid CountyId { get; } = countyId;
}
