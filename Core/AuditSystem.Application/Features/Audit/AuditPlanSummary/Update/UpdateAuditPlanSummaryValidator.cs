using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Update;

public sealed class UpdateAuditPlanSummaryValidator : AbstractValidator<UpdateAuditPlanSummaryCommand>
{
    public UpdateAuditPlanSummaryValidator()
    {
        RuleFor(x => x.Id)
             .NotEmpty()
             .WithMessage("Id is required.")
             .Must(x => x != Guid.Empty)
             .WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.Component)
            .NotEmpty()
            .WithMessage("Component is required.")
            .MinimumLength(2)
            .WithMessage("Component must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Component must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.ExampleDetails)
            .MaximumLength(500)
            .WithMessage("Example Details must not exceed 500 characters.");
    }
}
