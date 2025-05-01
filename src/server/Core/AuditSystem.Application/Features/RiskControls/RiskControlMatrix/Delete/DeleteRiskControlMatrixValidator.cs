namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Delete;

using FluentValidation;

public sealed class DeleteRiskControlMatrixValidator : AbstractValidator<DeleteRiskControlMatrixCommand>
{
    public DeleteRiskControlMatrixValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Risk Control Matrix Id is required")
           .Must(IsValidGuid)
           .WithMessage("Invalid Risk Control Matrix Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
