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

       
        RuleFor(x => x.Background)
            .NotEmpty()
            .WithMessage("Background is required.")
            .MinimumLength(2)
            .WithMessage("Background must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Background must not exceed 500 characters.");
      
        RuleFor(x => x.Highlight)
            .NotEmpty()
            .WithMessage("Highlight is required.")
            .MinimumLength(2)
            .WithMessage("Highlight must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Highlight must not exceed 500 characters.");
        
        RuleFor(x => x.IsActive)
            .NotNull()
            .WithMessage("IsActive is required.");
        
        RuleFor(x => x.Note)
            .MaximumLength(500)
            .WithMessage("Note must not exceed 500 characters.");
            
        RuleFor(x => x.AuditProgramId)
            .NotEmpty()
            .WithMessage("Audit Program Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Audit Program Id must not be empty.");
        
        RuleFor(x => x.RateId)
            .NotEmpty()
            .WithMessage("Rate Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Rate Id must be a valid GUID.");
    }
    
    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}