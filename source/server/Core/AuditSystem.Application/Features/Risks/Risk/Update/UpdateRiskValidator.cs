using FluentValidation;

namespace AuditSystem.Application.Features.Risks.Risk.Update;

public sealed class UpdateRiskValidator : AbstractValidator<UpdateRiskCommand>
{
    public UpdateRiskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Id must not be empty.");
        
        RuleFor(x => x.ObservationId)
            .NotEmpty()
            .WithMessage("ObservationId is required.")
            .Must(BeAValidGuid)
            .WithMessage("ObservationId must not be empty.");
        
        RuleFor(x => x.Factor)
            .NotEmpty()
            .WithMessage("Factor is required.")
            .MinimumLength(2)
            .WithMessage("Factor must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Factor must not exceed 100 characters.");
        
        RuleFor(x => x.Impact)
            .MaximumLength(500)
            .WithMessage("Impact must not exceed 500 characters.");
    }

    private static bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    } 
}