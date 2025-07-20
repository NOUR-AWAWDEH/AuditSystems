using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Delete;

public sealed class DeleteSubDepartmentValidator : AbstractValidator<DeleteSubDepartmentCommand>
{
    public DeleteSubDepartmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sub Department Id is required")
            .Must(IsValidId)
            .WithMessage("Sub Department Id is not valid");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(IsValidId)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool IsValidId(Guid id)
    {
        return id != Guid.Empty;
    }
}
