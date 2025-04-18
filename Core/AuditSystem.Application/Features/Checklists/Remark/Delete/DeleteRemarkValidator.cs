using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Remark.Delete;

public sealed class DeleteRemarkValidator : AbstractValidator<DeleteRemarkCommand>
{
    public DeleteRemarkValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Remark Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Remark Id cannot be empty.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}