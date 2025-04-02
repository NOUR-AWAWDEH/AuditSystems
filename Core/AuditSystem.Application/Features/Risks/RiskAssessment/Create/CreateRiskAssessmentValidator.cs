using FluentValidation;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Create;

public sealed class CreateRiskAssessmentValidator : AbstractValidator<CreateRiskAssessmentCommand>
{
    public CreateRiskAssessmentValidator() 
    {
        RuleFor(x => x.BusinessObjective)
            .NotEmpty()
            .WithMessage("Business Objective is required.")
            .MaximumLength(200)
            .WithMessage("Business Objective must not exceed 200 characters.");

        RuleFor(x => x.NatureThrough)
            .NotEmpty()
            .WithMessage("Nature Through is required.")
            .MaximumLength(200)
            .WithMessage("Nature Through must not exceed 200 characters.");

        RuleFor(x => x.PerformRiskAssessment)
            .NotEmpty()
            .WithMessage("Perform Risk Assessment is required.")
            .MaximumLength(200)
            .WithMessage("Perform Risk Assessment must not exceed 200 characters.");
    }
}
