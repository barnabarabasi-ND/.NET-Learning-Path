using System.ComponentModel.DataAnnotations;

namespace InsuranceApp.WebApi.Models.Buildings;

public class BuildingAddressRequest
{
    [Required]
    public Guid? CityId { get; init; }

    [Required]
    public string? Street { get; init; }
    
    [Required]
    public string? Number { get; init; }
}
