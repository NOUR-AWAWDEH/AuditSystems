namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Delete;
using FluentValidation;
public sealed class DeleteRiskProgramValidator : AbstractValidator<DeleteRiskProgramCommand>
{
    public DeleteRiskProgramValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Risk Program Id is required")
           .Must(IsValidGuid)
           .WithMessage("Invalid Risk Program Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
