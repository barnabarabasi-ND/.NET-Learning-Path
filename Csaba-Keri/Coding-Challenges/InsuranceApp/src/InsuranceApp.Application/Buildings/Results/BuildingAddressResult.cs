namespace InsuranceApp.Application.Buildings.Results;

public record BuildingAddressResult
{
    public Guid CityId { get; }
    public string Street { get; }
    public string Number { get; }

    public BuildingAddressResult(Guid cityId, string street, string number)
    {
        CityId = cityId;
        Street = street;
        Number = number;
    }
}
