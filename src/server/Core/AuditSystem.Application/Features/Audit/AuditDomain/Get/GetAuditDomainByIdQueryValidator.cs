using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Get;

public sealed class GetAuditDomainByIdQueryValidator : AbstractValidator<GetAuditDomainByIdQuery>
{
    public GetAuditDomainByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Domain Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Audit Domain Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }    
}