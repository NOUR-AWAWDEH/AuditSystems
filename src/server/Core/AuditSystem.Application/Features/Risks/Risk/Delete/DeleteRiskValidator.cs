namespace AuditSystem.Application.Features.Risks.Risk.Delete;
using FluentValidation;

public sealed class DeleteRiskValidator : AbstractValidator<DeleteRiskCommand>
{
    public DeleteRiskValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Risk Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Risk Id format");
    }
    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
