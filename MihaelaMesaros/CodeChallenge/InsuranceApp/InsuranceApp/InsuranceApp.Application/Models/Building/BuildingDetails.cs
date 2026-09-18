using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Models.Building;

internal sealed record BuildingDetails(
    int CityId,
    string? AddressStreet,
    string? AddressStreetNumber,
    int ConstructionYear,
    BuildingType BuildingType,
    int NumberOfFloors,
    decimal SurfaceArea,
    decimal InsuredValue,
    string? RiskIndicators
);
