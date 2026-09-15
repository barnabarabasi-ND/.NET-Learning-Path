using InsuranceApp.Application.Geography.Results;

namespace InsuranceApp.Application.Buildings.Results;

public record BuildingDetailsResult
{
    public BuildingResult Building { get; }
    public CityGeographyResult Geography { get; }

    public BuildingDetailsResult(BuildingResult building, CityGeographyResult geography)
    {
        Building = building;
        Geography = geography;
    }
}
