using InsuranceApp.Application.Common.Pagination;

namespace InsuranceApp.Application.Clients.Queries;

public record SearchClientsQuery
{
    public string? Name { get; init; }
    public string? Identifier { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public SearchClientsQuery(
        string? name = null,
        string? identifier = null,
        int pageNumber = PaginationDefaults.PageNumber,
        int pageSize = PaginationDefaults.PageSize
    )
    {
        Name = name;
        Identifier = identifier;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
