using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Search;

public class SearchSubDepartmentQueryValidator : AbstractValidator<SearchSubDepartmentQuery>
{
    public SearchSubDepartmentQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");

        RuleFor(x => x.DepartmentId)
            .Must(BeAValidGuid)
            .WithMessage("Department ID must be a valid GUID.");
    }
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
