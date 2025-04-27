namespace AuditSystem.Application.Features.Risks.RiskAssessment.Delete;
using FluentValidation;

public sealed class DeleteRiskAssessmentValidator : AbstractValidator<DeleteRiskAssessmentCommand> 
{
    public DeleteRiskAssessmentValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Risk Assessment Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Risk Assessment Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
