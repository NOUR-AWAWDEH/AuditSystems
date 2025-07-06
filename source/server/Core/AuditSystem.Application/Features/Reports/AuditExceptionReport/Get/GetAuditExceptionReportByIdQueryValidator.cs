using FluentValidation;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Get;

public sealed class GetAuditExceptionReportByIdQueryValidator : AbstractValidator<GetAuditExceptionReportByIdQuery>
{
    public GetAuditExceptionReportByIdQueryValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Audit Exception Report Id is required")
           .Must(BeAValidGuid)
           .WithMessage("Audit Exception Report Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }    
}
