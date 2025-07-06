using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Update;

public sealed class UpdateSubDepartmentValidator : AbstractValidator<UpdateSubDepartmentCommand>
{
    public UpdateSubDepartmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("DepartmentId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("DepartmentId must not be empty.");
    }
}
