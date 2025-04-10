using AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;
using FluentValidation;

public sealed class CreateAuditPlanSummaryValidator : AbstractValidator<CreateAuditPlanSummaryCommand>
{
    public CreateAuditPlanSummaryValidator()
    {

        RuleFor(x => x.Component)
            .NotEmpty()
            .WithMessage("The Component field is required.")
            .MinimumLength(2)
            .WithMessage("The Component must be at least 2 characters long.")
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