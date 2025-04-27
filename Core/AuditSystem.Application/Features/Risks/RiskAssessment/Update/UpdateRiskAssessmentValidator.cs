using FluentValidation;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Update;

public sealed class UpdateRiskAssessmentValidator : AbstractValidator<UpdateRiskAssessmentCommand>
{
    public UpdateRiskAssessmentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");
       
        RuleFor(x => x.BusinessObjective)
            .NotEmpty()
            .WithMessage("BusinessObjective is required.")
            .MinimumLength(2)
            .WithMessage("BusinessObjective must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("BusinessObjective must not exceed 100 characters.");
        
        RuleFor(x => x.NatureThrough)
            .NotEmpty()
            .WithMessage("NatureThrough is required.")
            .MinimumLength(2)
            .WithMessage("NatureThrough must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("NatureThrough must not exceed 100 characters.");
        
        RuleFor(x => x.PerformRiskAssessment)
            .NotEmpty()
            .WithMessage("PerformRiskAssessment is required.")
            .MinimumLength(2)
            .WithMessage("PerformRiskAssessment must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("PerformRiskAssessment must not exceed 100 characters.");
    }
}