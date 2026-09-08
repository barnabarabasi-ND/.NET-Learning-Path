namespace Domain.Entities;
public class City
{
    public Guid Id { get; private set; }
    public Guid CountyId { get; private set; }
    public string Name { get; private set; }
    public string PostalCode { get; private set; }

    public City(
        Guid countyId,
        string name,
        string postalCode)
    {
        Id = Guid.NewGuid();
        CountyId = countyId;
        Name = name;
        PostalCode = postalCode;
    }
}