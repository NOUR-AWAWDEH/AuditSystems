using FluentValidation;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Get;

public sealed class GetComplianceChecklistByIdQueryValidator : AbstractValidator<GetComplianceChecklistByIdQuery>
{
    public GetComplianceChecklistByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Compliance Checklist Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Compliance Checklist Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}