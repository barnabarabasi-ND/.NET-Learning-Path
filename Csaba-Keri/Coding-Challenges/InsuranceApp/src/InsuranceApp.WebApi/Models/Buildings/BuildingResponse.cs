namespace InsuranceApp.WebApi.Models.Buildings;

public class BuildingResponse(
    Guid id,
    Guid clientId,
    BuildingTypeDto type,
    BuildingAddressResponse address,
    int constructionYear,
    int numberOfFloors,
    decimal surfaceArea,
    decimal insuredValue
)
{
    public Guid Id { get; } = id;

    public Guid ClientId { get; } = clientId;

    public BuildingTypeDto Type { get; } = type;

    public BuildingAddressResponse Address { get; } = address;

    public int ConstructionYear { get; } = constructionYear;

    public int NumberOfFloors { get; } = numberOfFloors;

    public decimal SurfaceArea { get; } = surfaceArea;

    public decimal InsuredValue { get; } = insuredValue;
}
