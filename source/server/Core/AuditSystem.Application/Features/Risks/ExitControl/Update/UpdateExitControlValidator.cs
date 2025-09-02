using FluentValidation;

namespace AuditSystem.Application.Features.Risks.ExitControl.Update;

public sealed class UpdateExitControlValidator : AbstractValidator<UpdateExitControlCommand>
{
    public UpdateExitControlValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.RiskId)
            .NotEmpty()
            .WithMessage("Risk Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Risk Id must not be empty.");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MinimumLength(2)
            .WithMessage("Description must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
        
        RuleFor(x => x.Inadequate)
            .NotEmpty()
            .WithMessage("Inadequate is required.")
            .NotNull()
            .WithMessage("Inadequate is required.");
    }

    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    } 
}