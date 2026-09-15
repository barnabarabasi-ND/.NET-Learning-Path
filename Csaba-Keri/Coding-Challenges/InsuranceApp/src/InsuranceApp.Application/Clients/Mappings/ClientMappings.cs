using InsuranceApp.Application.Clients.Results;
using InsuranceApp.Domain.Clients;

namespace InsuranceApp.Application.Clients.Mappings;

internal static class ClientMappings
{
    public static ClientResult ToResult(this Client client)
    {
        return new(
            id: client.Id,
            type: client.Type,
            identificationNumber: client.IdentificationNumber,
            name: client.Name,
            email: client.Email,
            phone: client.Phone,
            primaryAddress: client.PrimaryAddress
        );
    }
}
