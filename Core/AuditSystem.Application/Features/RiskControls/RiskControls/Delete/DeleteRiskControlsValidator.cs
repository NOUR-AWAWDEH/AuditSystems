namespace AuditSystem.Application.Features.RiskControls.RiskControls.Delete;

using FluentValidation;

public sealed class DeleteRiskControlsValidator : AbstractValidator<DeleteRiskControlsCommand>
{
    public DeleteRiskControlsValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Risk Control Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Risk Control Id format");
    }
    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
