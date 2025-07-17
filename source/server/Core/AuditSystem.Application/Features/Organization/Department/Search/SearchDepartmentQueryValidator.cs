using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Search;

public class SearchDepartmentQueryValidator : AbstractValidator<SearchDepartmentQuery>
{
    public SearchDepartmentQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");

        RuleFor(x => x.SubLocationId)
            .NotEmpty()
            .WithMessage("SubLocation ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("SubLocation ID must be a valid GUID.");
    }
    
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}