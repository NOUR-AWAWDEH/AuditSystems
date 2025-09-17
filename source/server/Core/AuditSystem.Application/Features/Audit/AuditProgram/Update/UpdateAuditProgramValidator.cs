using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Update;

public sealed class UpdateAuditProgramValidator : AbstractValidator<UpdateAuditProgramCommand>
{
    public UpdateAuditProgramValidator()
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

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.ExitControlId)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Exit Control Id must not be empty.");
    }
    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}