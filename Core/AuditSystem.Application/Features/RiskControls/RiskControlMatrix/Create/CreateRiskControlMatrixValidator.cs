using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;

public sealed class CreateRiskControlMatrixValidator : AbstractValidator<CreateRiskControlMatrixCommand>
{
    public CreateRiskControlMatrixValidator() 
    {
        RuleFor(x => x.SubProcessId)
            .NotEmpty()
            .WithMessage("Sub Process Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Sub Process Id must be a valid GUID.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}
