namespace InsuranceApp.WebApi.Models.Buildings;

public class BuildingGeographyResponse(
    NamedLocationResponse country,
    NamedLocationResponse county,
    NamedLocationResponse city
)
{
    public NamedLocationResponse Country { get; } = country;

    public NamedLocationResponse County { get; } = county;

    public NamedLocationResponse City { get; } = city;
}
