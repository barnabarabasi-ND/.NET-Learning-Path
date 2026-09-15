namespace InsuranceApp.Application.Buildings.Commands;

public record BuildingAddressCommand
{
    public Guid CityId { get; init; }
    public string? Street { get; init; }
    public string? Number { get; init; }

    public BuildingAddressCommand(Guid cityId, string? street, string? number)
    {
        CityId = cityId;
        Street = street;
        Number = number;
    }
}
