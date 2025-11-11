using FluentValidation;

namespace AuditSystem.Application.Features.Observation.Create;

public sealed class CreateObservationCommandValidator : AbstractValidator<CreateObservationCommand>
{
    public CreateObservationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Observation Description is required.")
            .MinimumLength(2)
            .WithMessage("Observation Description must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Observation Description must not exceed 500 characters.");

        RuleFor(x => x.ActionOwner)
            .NotEmpty()
            .WithMessage("Action Owner is required.")
            .MinimumLength(2)
            .WithMessage("Action Owner must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Action Owner must not exceed 500 characters.");
        
        RuleFor(x => x.Background)
            .NotEmpty()
            .WithMessage("Background is required.")
            .MinimumLength(2)
            .WithMessage("Background must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Background must not exceed 500 characters.");
        
        RuleFor(x => x.ActionDate)
            .NotEmpty()
            .WithMessage("Action Date is required.");
        
        RuleFor(x => x.ActionPlan)
            .NotEmpty()
            .WithMessage("Action Plan is required.")
            .MinimumLength(2)
            .WithMessage("Action Plan must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Action Plan must not exceed 500 characters.");
        
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage("Note must not exceed 500 characters.");
        
        RuleFor(x => x.AuditProgramId)
            .NotEmpty()
            .WithMessage("Audit Program Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Audit Program Id must be a valid GUID.");
    }
}
