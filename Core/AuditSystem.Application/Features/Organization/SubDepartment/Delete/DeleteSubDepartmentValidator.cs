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
    }

    private bool IsValidId(Guid id)
    {
        return id != Guid.Empty;
    }
}
