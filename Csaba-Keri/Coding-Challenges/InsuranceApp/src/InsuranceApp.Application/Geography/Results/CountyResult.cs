namespace InsuranceApp.Application.Geography.Results;

public record CountyResult
{
    public Guid Id { get; }
    public string Name { get; }
    public Guid CountryId { get; }

    public CountyResult(Guid id, string name, Guid countryId)
    {
        Id = id;
        Name = name;
        CountryId = countryId;
    }
}
