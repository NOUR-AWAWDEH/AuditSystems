using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Create;

public sealed class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department Name is required.")
            .MinimumLength(2)
            .WithMessage("Department Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Department Name must not exceed 200 characters.");

        RuleFor(x => x.SubLocationId)
            .NotEmpty()
            .WithMessage("Sublocation Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Sublocation Id must be a valid GUID.");

    }
}
