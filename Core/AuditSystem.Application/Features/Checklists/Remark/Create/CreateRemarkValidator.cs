using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Remark.Create;

public sealed class CreateRemarkValidator : AbstractValidator<CreateRemarkCommand>
{
    public CreateRemarkValidator() 
    {
        RuleFor(x => x.CheckListManagementId)
            .NotEmpty()
            .WithMessage("CheckList Management Id is required");

        RuleFor(x => x.Remarkcommants)
            .NotEmpty()
            .WithMessage("Remark comments are required")
            .MaximumLength(500)
            .WithMessage("Remark comments must not exceed 500 characters");
    }
}
