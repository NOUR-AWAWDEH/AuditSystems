using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Checklist.Create;

public sealed class CreateChecklistValidator : AbstractValidator<CreateChecklistCommand>
{
    public CreateChecklistValidator()
    {
        RuleFor(x => x.Area)
            .NotEmpty()
            .WithMessage("Area is required")
            .MinimumLength(2)
            .WithMessage("Area name must be at least 2 characters long")
            .MaximumLength(300)
            .WithMessage("Area name is too long");

        RuleFor(x => x.Particulars)
           .NotEmpty()
           .WithMessage("Particulars is required")
           .MinimumLength(2)
           .WithMessage("Particulars name must be at least 2 characters long")
           .MaximumLength(300)
           .WithMessage("Particulars name is too long");

        RuleFor(x => x.Observation)
           .NotEmpty()
           .WithMessage("Observation is required")
           .MinimumLength(2)
           .WithMessage("Observation must be at least 2 characters long")
           .MaximumLength(300)
           .WithMessage("Observation cannot exceed 300 characters");


        RuleFor(x => x.ChecklistCollectionId)
            .NotEmpty()
            .WithMessage("Checklist Collection Id is required")
            .Must(x => x != Guid.Empty)
            .WithMessage("Checklist Collection Id must be a valid GUID");

    }
}