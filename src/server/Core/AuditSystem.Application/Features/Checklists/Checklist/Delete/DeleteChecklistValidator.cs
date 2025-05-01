using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Checklist.Delete;

public sealed class DeleteChecklistValidator : AbstractValidator<DeleteChecklistCommand>
{
    public DeleteChecklistValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Checklist ID is required.")
            .Must(IsValidGuid)
            .WithMessage("Checklist ID cannot be an empty GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}