using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Create;

public sealed class CreateSubDepartmentValidator : AbstractValidator<CreateSubDepartmentCommand>
{
    public CreateSubDepartmentValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Department Id must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("SubDepartment Name is required.")
            .MinimumLength(2)
            .WithMessage("SubDepartment Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("SubDepartment Name must not exceed 200 characters.");
    }
}
