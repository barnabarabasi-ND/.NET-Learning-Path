using InsuranceApp.Domain.Buildings;

namespace InsuranceApp.Application.Buildings.Results;

public record BuildingResult
{
    public Guid Id { get; }
    public Guid ClientId { get; }
    public BuildingType Type { get; }
    public BuildingAddressResult Address { get; }
    public int ConstructionYear { get; }
    public int NumberOfFloors { get; }
    public decimal SurfaceArea { get; }
    public decimal InsuredValue { get; }

    public BuildingResult(
        Guid id,
        Guid clientId,
        BuildingType type,
        BuildingAddressResult address,
        int constructionYear,
        int numberOfFloors,
        decimal surfaceArea,
        decimal insuredValue
    )
    {
        Id = id;
        ClientId = clientId;
        Type = type;
        Address = address;
        ConstructionYear = constructionYear;
        NumberOfFloors = numberOfFloors;
        SurfaceArea = surfaceArea;
        InsuredValue = insuredValue;
    }
}
