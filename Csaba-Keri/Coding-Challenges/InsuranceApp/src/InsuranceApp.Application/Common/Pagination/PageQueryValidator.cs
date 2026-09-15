using FluentValidation;

namespace InsuranceApp.Application.Common.Pagination;

public class PageQueryValidator : AbstractValidator<PageQuery>
{
    public PageQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, PaginationDefaults.MaxPageSize);
    }
}
