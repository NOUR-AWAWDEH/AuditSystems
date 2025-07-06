using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Get;

public sealed class GetAuditEngagementByIdQueryValidator : AbstractValidator<GetAuditEngagementByIdQuery>
{
    public GetAuditEngagementByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Engagement Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Engagement Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty; 
    }
}