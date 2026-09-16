using FluentValidation;
using InsuranceApp.Application.Buildings.Commands;
using InsuranceApp.Application.Buildings.Results;
using InsuranceApp.Application.Clients.Commands;
using InsuranceApp.Application.Clients.Queries;
using InsuranceApp.Application.Clients.Results;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Application.Geography.Results;
using InsuranceApp.Domain.Buildings;
using InsuranceApp.Domain.Clients;
using InsuranceApp.WebApi.Models.Buildings;
using InsuranceApp.WebApi.Models.Clients;
using InsuranceApp.WebApi.Models.Common;
using InsuranceApp.WebApi.Models.Geography;

namespace InsuranceApp.WebApi.Mappings;

internal static class ApiMappings
{
    public static ClientType ToDomain(this ClientTypeDto type)
    {
        return type switch
        {
            ClientTypeDto.Individual => ClientType.Individual,
            ClientTypeDto.Company => ClientType.Company,

            _ => throw new ValidationException("Client type is invalid.")
        };
    }

    public static BuildingType ToDomain(this BuildingTypeDto type)
    {
        return type switch
        {
            BuildingTypeDto.Residential => BuildingType.Residential,
            BuildingTypeDto.Office => BuildingType.Office,
            BuildingTypeDto.Industrial => BuildingType.Industrial,

            _ => throw new ValidationException("Building type is invalid.")
        };
    }

    public static ClientTypeDto ToDto(this ClientType type)
    {
        return type switch
        {
            ClientType.Individual => ClientTypeDto.Individual,
            ClientType.Company => ClientTypeDto.Company,

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static BuildingTypeDto ToDto(this BuildingType type)
    {
        return type switch
        {
            BuildingType.Residential => BuildingTypeDto.Residential,
            BuildingType.Office => BuildingTypeDto.Office,
            BuildingType.Industrial => BuildingTypeDto.Industrial,

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static CreateClientCommand ToCommand(this CreateClientRequest request)
    {
        return new(
            type: request.Type!.Value.ToDomain(),
            identificationNumber: request.IdentificationNumber,
            name: request.Name,
            email: request.Email,
            phone: request.Phone,
            primaryAddress: request.PrimaryAddress
        );
    }

    public static UpdateClientCommand ToCommand(this UpdateClientRequest request, Guid clientId)
    {
        return new(
            clientId: clientId,
            name: request.Name,
            email: request.Email,
            phone: request.Phone,
            primaryAddress: request.PrimaryAddress
        );
    }

    public static CreateBuildingCommand ToCreateCommand(this SaveBuildingRequest request, Guid clientId)
    {
        return new(
            clientId: clientId,
            type: request.Type!.Value.ToDomain(),
            address: request.Address!.ToCommand(),
            constructionYear: request.ConstructionYear!.Value,
            numberOfFloors: request.NumberOfFloors!.Value,
            surfaceArea: request.SurfaceArea!.Value,
            insuredValue: request.InsuredValue!.Value
        );
    }

    public static UpdateBuildingCommand ToUpdateCommand(this SaveBuildingRequest request, Guid buildingId)
    {
        return new(
            buildingId: buildingId,
            type: request.Type!.Value.ToDomain(),
            address: request.Address!.ToCommand(),
            constructionYear: request.ConstructionYear!.Value,
            numberOfFloors: request.NumberOfFloors!.Value,
            surfaceArea: request.SurfaceArea!.Value,
            insuredValue: request.InsuredValue!.Value
        );
    }

    private static BuildingAddressCommand ToCommand(this BuildingAddressRequest request)
    {
        return new(
            cityId: request.CityId!.Value,
            street: request.Street,
            number: request.Number
        );
    }

    public static PageQuery ToQuery(this PageRequest request)
    {
        return new(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize
        );
    }

    public static SearchClientsQuery ToQuery(this SearchClientsRequest request)
    {
        return new(
            name: request.Name,
            identifier: request.Identifier,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize
        );
    }

    public static ClientResponse ToResponse(this ClientResult result)
    {
        return new(
            id: result.Id,
            type: result.Type.ToDto(),
            identificationNumber: result.IdentificationNumber,
            name: result.Name,
            email: result.Email,
            phone: result.Phone,
            primaryAddress: result.PrimaryAddress
        );
    }

    public static BuildingResponse ToResponse(this BuildingResult result)
    {
        return new(
            id: result.Id,
            clientId: result.ClientId,
            type: result.Type.ToDto(),
            address: result.Address.ToResponse(),
            constructionYear: result.ConstructionYear,
            numberOfFloors: result.NumberOfFloors,
            surfaceArea: result.SurfaceArea,
            insuredValue: result.InsuredValue
        );
    }

    public static BuildingDetailsResponse ToResponse(this BuildingDetailsResult result)
    {
        var building = result.Building;
        var geography = result.Geography;

        return new(
            id: building.Id,
            clientId: building.ClientId,
            type: building.Type.ToDto(),
            address: building.Address.ToResponse(),
            constructionYear: building.ConstructionYear,
            numberOfFloors: building.NumberOfFloors,
            surfaceArea: building.SurfaceArea,
            insuredValue: building.InsuredValue,
            geography: new(
                country: new(geography.Country.Id, geography.Country.Name),
                county: new(geography.County.Id, geography.County.Name),
                city: new(geography.City.Id, geography.City.Name)
            )
        );
    }

    public static BuildingAddressResponse ToResponse(this BuildingAddressResult result)
    {
        return new(
            cityId: result.CityId,
            street: result.Street,
            number: result.Number
        );
    }

    public static CountryResponse ToResponse(this CountryResult result)
    {
        return new(
            id: result.Id,
            name: result.Name
        );
    }

    public static CountyResponse ToResponse(this CountyResult result)
    {
        return new(
            id: result.Id,
            name: result.Name,
            countryId: result.CountryId
        );
    }

    public static CityResponse ToResponse(this CityResult result)
    {
        return new(
            id: result.Id,
            name: result.Name,
            countyId: result.CountyId
        );
    }

    public static PagedResponse<TResponse> ToResponse<TResult, TResponse>(
        this PagedResult<TResult> result,
        Func<TResult, TResponse> toResponse
    )
    {
        return new(
            items: [.. result.Items.Select(toResponse)],
            pageNumber: result.PageNumber,
            pageSize: result.PageSize,
            totalCount: result.TotalCount
        );
    }
}
