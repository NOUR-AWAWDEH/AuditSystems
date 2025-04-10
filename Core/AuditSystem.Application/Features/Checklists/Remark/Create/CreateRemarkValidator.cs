using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Remark.Create;

public sealed class CreateRemarkValidator : AbstractValidator<CreateRemarkCommand>
{
    public CreateRemarkValidator() 
    {

        RuleFor(x => x.RemarkCommants)
            .NotEmpty()
            .WithMessage("Remark comments are required")
            .MaximumLength(500)
            .WithMessage("Remark comments must not exceed 500 characters");
    }
}
