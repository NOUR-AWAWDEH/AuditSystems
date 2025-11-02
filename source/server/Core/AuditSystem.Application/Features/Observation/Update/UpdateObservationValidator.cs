using FluentValidation;

namespace AuditSystem.Application.Features.Observation.Update;

public sealed class UpdateObservationValidator : AbstractValidator<UpdateObservationCommand>
{
    public UpdateObservationValidator()
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
            
        RuleFor(x => x.ActionPlan)
            .NotEmpty()
            .WithMessage("Action Plan is required.")
            .MaximumLength(500)
            .WithMessage("Action Plan must not exceed 500 characters.");
            
        RuleFor(x => x.ActionDate)
            .NotEmpty()
            .WithMessage("Action Date is required.");
            
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage("Note must not exceed 500 characters.");
            
        RuleFor(x => x.AuditProgramId)
            .NotEmpty()
            .WithMessage("Audit Program Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Audit Program Id must not be empty.");
    }
    
    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}