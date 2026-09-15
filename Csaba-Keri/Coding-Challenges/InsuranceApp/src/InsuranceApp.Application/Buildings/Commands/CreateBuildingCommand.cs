using InsuranceApp.Domain.Buildings;

namespace InsuranceApp.Application.Buildings.Commands;

public record CreateBuildingCommand : IBuildingDetailsCommand
{
    public Guid ClientId { get; init; }
    public BuildingType Type { get; init; }
    public BuildingAddressCommand? Address { get; init; }
    public int ConstructionYear { get; init; }
    public int NumberOfFloors { get; init; }
    public decimal SurfaceArea { get; init; }
    public decimal InsuredValue { get; init; }

    public CreateBuildingCommand(
        Guid clientId,
        BuildingType type,
        BuildingAddressCommand? address,
        int constructionYear,
        int numberOfFloors,
        decimal surfaceArea,
        decimal insuredValue
    )
    {
        ClientId = clientId;
        Type = type;
        Address = address;
        ConstructionYear = constructionYear;
        NumberOfFloors = numberOfFloors;
        SurfaceArea = surfaceArea;
        InsuredValue = insuredValue;
    }
}
