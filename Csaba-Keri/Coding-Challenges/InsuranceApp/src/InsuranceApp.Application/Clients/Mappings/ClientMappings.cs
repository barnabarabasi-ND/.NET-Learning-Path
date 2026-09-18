using InsuranceApp.Application.Clients.Results;
using InsuranceApp.Application.Common.Pagination;
using InsuranceApp.Domain.Clients;

namespace InsuranceApp.Application.Clients.Mappings;

internal static class ClientMappings
{
    public static ClientResult ToResult(this Client client)
    {
        return new(
            Id: client.Id,
            Type: client.Type,
            IdentificationNumber: client.IdentificationNumber,
            Name: client.Name,
            Email: client.Email,
            Phone: client.Phone,
            PrimaryAddress: client.PrimaryAddress
        );
    }

    public static PagedResult<ClientResult> ToResult(this PagedResult<Client> page)
    {
        return new(
            items: page.Items.Select(client => client.ToResult()),
            pageNumber: page.PageNumber,
            pageSize: page.PageSize,
            totalCount: page.TotalCount
        );
    }
}
