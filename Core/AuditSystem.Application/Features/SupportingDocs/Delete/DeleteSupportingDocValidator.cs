namespace AuditSystem.Application.Features.SupportingDocs.Delete;
using FluentValidation;

public sealed class DeleteSupportingDocValidator : AbstractValidator<DeleteSupportingDocCommand>
{
    public DeleteSupportingDocValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Supporting Doc Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Supporting Doc Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
