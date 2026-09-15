namespace InsuranceApp.Application.Geography.Results;

public record CityResult
{
    public Guid Id { get; }
    public string Name { get; }
    public Guid CountyId { get; }

    public CityResult(Guid id, string name, Guid countyId)
    {
        Id = id;
        Name = name;
        CountyId = countyId;
    }
}
