using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Update;

public sealed class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.SublocationId)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("SubLocation Id must not be empty.");
    }
    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}