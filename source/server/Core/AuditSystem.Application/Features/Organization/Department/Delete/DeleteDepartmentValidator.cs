using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Delete;

public sealed class DeleteDepartmentValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Department ID is required.")
            .Must(IsValidGuid)
            .WithMessage("Department ID must be a valid GUID.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(IsValidGuid)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
