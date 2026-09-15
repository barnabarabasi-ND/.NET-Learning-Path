namespace InsuranceApp.Application.Geography.Results;

public record CountryResult
{
    public Guid Id { get; }
    public string Name { get; }

    public CountryResult(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
