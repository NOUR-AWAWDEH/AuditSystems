using FluentValidation;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Update;

public sealed class UpdateComplianceChecklistValidator : AbstractValidator<UpdateComplianceChecklistCommand>
{
    public UpdateComplianceChecklistValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.Area)
            .NotEmpty()
            .WithMessage("Area is required.")
            .MaximumLength(100)
            .WithMessage("Area must not exceed 100 characters.");

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject is required.")
            .MaximumLength(100)
            .WithMessage("Subject must not exceed 100 characters.");

        RuleFor(x => x.Particulars)
            .NotEmpty()
            .WithMessage("Particulars are required.")
            .MaximumLength(500)
            .WithMessage("Particulars must not exceed 500 characters.");
    }
}

