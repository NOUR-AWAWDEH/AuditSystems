using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;

public sealed class CreateRiskControlMatrixValidator : AbstractValidator<CreateRiskControlMatrixCommand>
{
    public CreateRiskControlMatrixValidator() 
    {
        RuleFor(x => x.SubProcessId)
            .NotEmpty()
            .WithMessage("Sub Process Id is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}
