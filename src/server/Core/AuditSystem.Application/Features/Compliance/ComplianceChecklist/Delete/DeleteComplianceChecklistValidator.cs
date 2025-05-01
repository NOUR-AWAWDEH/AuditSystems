using FluentValidation;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Delete;

public sealed class DeleteComplianceChecklistValidator : AbstractValidator<DeleteComplianceChecklistCommand>
{
    public DeleteComplianceChecklistValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Compliance Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Compliance Id cannot be empty.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
