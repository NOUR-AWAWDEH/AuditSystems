using FluentValidation;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Delete;

public sealed class DeleteSpecialProjectValidator : AbstractValidator<DeleteSpecialProjectCommand>
{
    public DeleteSpecialProjectValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Special Project Id is required.")
           .Must(IsValidGuid)
           .WithMessage("Special Project Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}