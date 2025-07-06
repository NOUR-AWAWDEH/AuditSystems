using FluentValidation;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Update;

public sealed class UpdateAuditProcessValidator : AbstractValidator<UpdateAuditProcessCommand>
{
    public UpdateAuditProcessValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.ProcessName)
            .NotEmpty()
            .WithMessage("ProcessName is required.")
            .MinimumLength(2)
            .WithMessage("ProcessName must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("ProcessName must not exceed 100 characters.");

        RuleFor(x => x.RatingId)
            .NotEmpty()
            .WithMessage("RatingId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("RatingId must not be empty.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}