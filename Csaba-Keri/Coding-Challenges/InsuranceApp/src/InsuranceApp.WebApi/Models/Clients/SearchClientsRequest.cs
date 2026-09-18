using InsuranceApp.Application.Common.Pagination;

namespace InsuranceApp.WebApi.Models.Clients;

public record SearchClientsRequest(
    string? Name,
    string? Identifier,
    int PageNumber = PaginationDefaults.PageNumber,
    int PageSize = PaginationDefaults.PageSize
);
