using FluentValidation;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Create;

public sealed class CreateSpecialProjectValidator : AbstractValidator<CreateSpecialProjectCommand>
{
    public CreateSpecialProjectValidator()
    {
        RuleFor(x => x.AuditUniverseId)
            .NotEmpty()
            .WithMessage("Audit Universe Id is required");

        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .WithMessage("Project Name is required")
            .MinimumLength(2)
            .WithMessage("Project Name must be at least 2 characters long")
            .MaximumLength(200)
            .WithMessage("Project Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start Date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End Date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End Date must be after Start Date.");
    }
}