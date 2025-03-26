using FluentValidation;

namespace AuditSystem.Application.Features.Organisation.SubDepartment.Create;

public sealed class CreateSubDepartmentValidator : AbstractValidator<CreateSubDepartmentCommand> 
{
    public CreateSubDepartmentValidator() 
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("SubDepartment Name is required.")
            .MaximumLength(200)
            .WithMessage("SubDepartment Name must not exceed 200 characters.");
    }
}
