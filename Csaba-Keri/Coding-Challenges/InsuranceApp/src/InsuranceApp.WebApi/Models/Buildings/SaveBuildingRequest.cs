using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.WebApi.Models.Buildings;

public class SaveBuildingRequest
{
    [Required]
    public BuildingTypeDto? Type { get; init; }

    [Required]
    public BuildingAddressRequest? Address { get; init; }

    [Required]
    public int? ConstructionYear { get; init; }

    [Required]
    public int? NumberOfFloors { get; init; }

    [Required]
    public decimal? SurfaceArea { get; init; }

    [Required]
    public decimal? InsuredValue { get; init; }
}
