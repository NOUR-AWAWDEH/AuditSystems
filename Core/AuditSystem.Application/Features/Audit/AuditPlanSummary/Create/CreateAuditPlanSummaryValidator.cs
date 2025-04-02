using FluentValidation;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;

public sealed class CreateAuditPlanSummaryValidator : AbstractValidator<CreateAuditPlanSummaryCommand>
{
    public CreateAuditPlanSummaryValidator()
    {
        RuleFor(x => x.AuditorSettingsId)
            .NotEmpty()
            .WithMessage("The AuditorSettingsId field is required.");

        RuleFor(x => x.Component)
            .MaximumLength(255)
            .WithMessage("The Component must not exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("The Description must not exceed 500 characters.");

        RuleFor(x => x.ExampleDetails)
            .MaximumLength(255)
            .WithMessage("The Example details must not exceed 255 characters.");
    }
}