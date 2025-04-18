using FluentValidation;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Update;

public sealed class UpdateSpecialProjectValidator : AbstractValidator<UpdateSpecialProjectCommand>
{
    public UpdateSpecialProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.AuditUniverseId)
            .NotEmpty()
            .WithMessage("Audit Universe ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Audit Universe ID must not be empty.");

        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .WithMessage("Project Name is required.")
            .MinimumLength(2)
            .WithMessage("Project Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Project Name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start Date is required.")
            .Must(x => x != default)
            .WithMessage("Start Date must not be empty.")
            .Must(x => x >= DateTime.UtcNow)
            .WithMessage("Start Date Must not be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End Date is required.")
            .Must(x => x == null || x > DateTime.UtcNow)
            .WithMessage("End Date must be in the future or null.");
    }
}
