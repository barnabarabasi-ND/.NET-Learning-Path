using InsuranceApp.Application.Buildings.Commands;
using InsuranceApp.Application.Buildings.Results;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Application.Geography.Results;
using InsuranceApp.Domain.Buildings;

namespace InsuranceApp.Application.Buildings.Mappings;

internal static class BuildingMappings
{
    public static BuildingAddress ToDomain(this BuildingAddressCommand command)
    {
        return new(
            cityId: command.CityId,
            street: command.Street!,
            number: command.Number!
        );
    }

    public static BuildingAddressResult ToResult(this BuildingAddress buildingAddress)
    {
        return new(
            CityId: buildingAddress.CityId,
            Street: buildingAddress.Street,
            Number: buildingAddress.Number
        );
    }

    public static BuildingResult ToResult(this Building building)
    {
        return new(
            Id: building.Id,
            ClientId: building.ClientId,
            Type: building.Type,
            Address: building.Address.ToResult(),
            ConstructionYear: building.ConstructionYear,
            NumberOfFloors: building.NumberOfFloors,
            SurfaceArea: building.SurfaceArea,
            InsuredValue: building.InsuredValue
        );
    }

    public static PagedResult<BuildingResult> ToResult(this PagedResult<Building> page)
    {
        return new(
            items: page.Items.Select(building => building.ToResult()),
            pageNumber: page.PageNumber,
            pageSize: page.PageSize,
            totalCount: page.TotalCount
        );
    }

    public static BuildingDetailsResult ToDetailsResult(this Building building, CityGeographyResult geography)
    {
        return new(
            Building: building.ToResult(),
            Geography: geography
        );
    }
}
