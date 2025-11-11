using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;
using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Search;

public class SearchDepartmentQueryValidator : PaginationRequestValidator<DepartmentModel, SearchDepartmentQuery>
{
    public SearchDepartmentQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");

        RuleFor(x => x.LocationId)
            .Must(BeAValidGuid)
            .WithMessage("Location ID must be a valid GUID");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term must not exceed 200 characters.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}