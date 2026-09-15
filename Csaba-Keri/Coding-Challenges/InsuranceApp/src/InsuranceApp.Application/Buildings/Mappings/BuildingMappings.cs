using InsuranceApp.Application.Buildings.Results;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Domain.Buildings;

namespace InsuranceApp.Application.Buildings.Mappings;

internal static class BuildingMappings
{
    public static BuildingAddressResult ToResult(this BuildingAddress buildingAddress)
    {
        return new(
            cityId: buildingAddress.CityId,
            street: buildingAddress.Street,
            number: buildingAddress.Number
        );
    }

    public static BuildingResult ToResult(this Building building)
    {
        return new(
            id: building.Id,
            clientId: building.ClientId,
            type: building.Type,
            address: building.Address.ToResult(),
            constructionYear: building.ConstructionYear,
            numberOfFloors: building.NumberOfFloors,
            surfaceArea: building.SurfaceArea,
            insuredValue: building.InsuredValue
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
}
