using FluentValidation;

namespace AuditSystem.Application.Features.SupportingDocs.Create;

public sealed class CreateSupportingDocValidator : AbstractValidator<CreateSupportingDocCommand>
{
    public CreateSupportingDocValidator() 
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("Supporting Doc File Name is required.")
            .MaximumLength(200)
            .WithMessage("Supporting Doc File Name must not exceed 200 characters.");

        RuleFor(x => x.FileSize)
            .NotEmpty()
            .WithMessage("Supporting Doc File Size is required.")
            .GreaterThan(0)
            .WithMessage("Supporting Doc File Size must be greater than 0.");

        RuleFor(x => x.URL)
            .NotEmpty()
            .WithMessage("Supporting Doc URL is required.")
            .MaximumLength(500)
            .WithMessage("Supporting Doc URL must not exceed 500 characters.");
    }
}
