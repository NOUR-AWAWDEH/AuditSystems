using FluentValidation;

namespace AuditSystem.Application.Features.Checklists.Checklist.Create;

public sealed class CreateChecklistValidator : AbstractValidator<CreateChecklistCommand>
{
    public CreateChecklistValidator() 
    {
        RuleFor(x => x.Area)
            .NotEmpty()
            .WithMessage("Area is required")
            .MaximumLength(300)
            .WithMessage("Area name is too long");
        
        RuleFor(x => x.Particulars)
          .NotEmpty()
         .WithMessage("Particulars is required")
        .MaximumLength(300)
        .WithMessage("Particulars name is too long");

        RuleFor(x => x.Observation)
        .MaximumLength(300)
        .WithMessage("Observation cannot exceed 300 characters");
    }   
}