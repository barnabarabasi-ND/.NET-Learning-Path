namespace InsuranceApp.WebApi.Models.Clients;

public class SearchClientsRequest
{
    public string? Name { get; init; }
    
    public string? Identifier { get; init; }
    
    public int PageNumber { get; init; } = 1;
    
    public int PageSize { get; init; } = 20;
}
