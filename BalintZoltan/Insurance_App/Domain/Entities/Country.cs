namespace Domain.Entities;
public class Country
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private readonly List<County> _counties = new();

    public IReadOnlyCollection<County> Counties => _counties;

    public Country(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}