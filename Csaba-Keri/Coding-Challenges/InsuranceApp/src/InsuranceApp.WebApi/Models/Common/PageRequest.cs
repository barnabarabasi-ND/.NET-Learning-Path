namespace InsuranceApp.WebApi.Models.Common;

public class PageRequest
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 20;
}
