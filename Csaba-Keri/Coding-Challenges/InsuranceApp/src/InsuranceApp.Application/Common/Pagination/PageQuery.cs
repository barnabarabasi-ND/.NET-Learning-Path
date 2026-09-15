namespace InsuranceApp.Application.Common.Pagination;

public record PageQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public PageQuery(
        int pageNumber = PaginationDefaults.PageNumber,
        int pageSize = PaginationDefaults.PageSize
    )
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
