namespace InsuranceApp.WebApi.Models.Buildings;

public class BuildingAddressResponse(Guid cityId, string street, string number)
{
    public Guid CityId { get; } = cityId;
    
    public string Street { get; } = street;
    
    public string Number { get; } = number;
}
