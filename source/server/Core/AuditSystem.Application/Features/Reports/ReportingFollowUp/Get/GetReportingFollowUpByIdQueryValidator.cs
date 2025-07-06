using FluentValidation;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Get;

public sealed class GetReportingFollowUpByIdQueryValidator : AbstractValidator<GetReportingFollowUpByIdQuery>
{
    public GetReportingFollowUpByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Reporting Follow Up Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Reporting Follow Up Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id!= Guid.Empty;
    }    
}