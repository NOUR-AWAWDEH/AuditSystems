using FluentValidation;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;

public sealed class CreateComplianceChecklistValidator : AbstractValidator<CreateComplianceChecklistCommand>
{
    public CreateComplianceChecklistValidator()
    {
        RuleFor(x => x.Area)
            .NotEmpty()
            .WithMessage("The Area field is required.")
            .MaximumLength(100)
            .WithMessage("The Area field must not exceed 100 characters.");

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("The Subject field is required.")
            .MaximumLength(300)
            .WithMessage("The Subject field must not exceed 300 characters.");

        RuleFor(x => x.Particulars)
            .NotEmpty()
            .WithMessage("The Particulars field is required.")
            .MaximumLength(500)
            .WithMessage("The Particulars field must not exceed 500 characters.");
    }
}