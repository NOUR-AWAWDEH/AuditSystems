using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Remark.Update;

public sealed class UpdateRemarkValidator : AbstractValidator<UpdateRemarkCommand>
{
    public UpdateRemarkValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.RemarkCommants)
            .NotEmpty()
            .WithMessage("Remark comments are required.")
            .MinimumLength(2)
            .WithMessage("Remark comments must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Remark comments cannot exceed 500 characters.");

        RuleFor(x => x.CheckListCollectionId)
            .NotEmpty()
            .WithMessage("Checklist collection ID is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Checklist collection ID must be a valid GUID.");
    }
}
