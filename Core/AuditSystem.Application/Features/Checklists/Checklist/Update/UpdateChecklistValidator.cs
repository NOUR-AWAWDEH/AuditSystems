using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Checklist.Update;

public sealed class UpdateChecklistValidator : AbstractValidator<UpdateChecklistCommand>
{
    public UpdateChecklistValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Checklist ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Checklist ID must be a valid GUID.");

        RuleFor(x => x.Area)
            .NotEmpty()
            .WithMessage("Area is required.")
            .MinimumLength(2)
            .WithMessage("Area must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Area must not exceed 200 characters.");

        RuleFor(x => x.Particulars)
            .NotEmpty()
            .WithMessage("Particulars are required.")
            .MinimumLength(2)
            .WithMessage("Particulars must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Particulars must not exceed 200 characters.");

        RuleFor(x => x.Observation)
            .NotEmpty()
            .WithMessage("Observation is required.")
            .MinimumLength(2)
            .WithMessage("Observation must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Observation must not exceed 200 characters.");

        RuleFor(x => x.ChecklistCollectionId)
            .NotEmpty()
            .WithMessage("Checklist Collection ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Checklist Collection ID must be a valid GUID.");
    }
}
