using FluentValidation;
using InsuranceApp.Application.Clients.Queries;
using InsuranceApp.Application.Common.Pagination;

namespace InsuranceApp.Application.Clients.Validation;

public class SearchClientsQueryValidator : AbstractValidator<SearchClientsQuery>
{
    public SearchClientsQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, PaginationDefaults.MaxPageSize)
            .WithMessage($"Page size must be between 1 and {PaginationDefaults.MaxPageSize}.");
    }
}
