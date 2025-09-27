using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Update;

public sealed class UpdateControlDesignValidator : AbstractValidator<UpdateControlDesignCommand>
{
    public UpdateControlDesignValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.ExitControlId)
            .NotEmpty()
            .WithMessage("Exit Control Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Exit Control Id must not be empty.");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MinimumLength(2)
            .WithMessage("Description must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
        
        RuleFor(x => x.Inadequate)
            .NotNull()
            .WithMessage("Inadequate is required.");
    }

    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    } 
}