namespace Domain.Entities;
public class County
{
    public Guid Id { get; private set; }
    public Guid CountryId { get; private set; }
    public string Name { get; private set; }

    private readonly List<City> _cities = new();

    public IReadOnlyCollection<City> Cities => _cities;

    public County(Guid countryId, string name)
    {
        Id = Guid.NewGuid();
        CountryId = countryId;
        Name = name;
    }
}