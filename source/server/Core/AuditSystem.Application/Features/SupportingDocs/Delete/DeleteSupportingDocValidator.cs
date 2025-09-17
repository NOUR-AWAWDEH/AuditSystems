namespace AuditSystem.Application.Features.SupportingDocs.Delete;
using FluentValidation;

public sealed class DeleteSupportingDocValidator : AbstractValidator<DeleteSupportingDocCommand>
{
    public DeleteSupportingDocValidator()
    {
        RuleFor(x => x.Id)
              .NotEmpty()
              .WithMessage("Supporting Document Id is required.")
              .Must(IsValidGuid)
              .WithMessage("Supporting Document Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
