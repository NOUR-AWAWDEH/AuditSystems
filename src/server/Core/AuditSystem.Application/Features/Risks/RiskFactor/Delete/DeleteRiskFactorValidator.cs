namespace AuditSystem.Application.Features.Risks.RiskFactor.Delete;
using FluentValidation;

public sealed class DeleteRiskFactorValidator : AbstractValidator<DeleteRiskFactorCommand>
{
    public DeleteRiskFactorValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Risk Factor Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Risk Factor Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
