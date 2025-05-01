using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Delete;

public sealed class DeleteAuditEngagementValidator : AbstractValidator<DeleteAuditEngagementCommand>
{
    public DeleteAuditEngagementValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Engagement Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Audit Engagement Id is not a valid GUID.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}
