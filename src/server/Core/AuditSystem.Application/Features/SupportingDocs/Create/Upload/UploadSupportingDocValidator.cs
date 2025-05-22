using FluentValidation;

namespace AuditSystem.Application.Features.SupportingDocs.Create.Uplode;

public class UploadSupportingDocValidator : AbstractValidator<UploadSupportingDocCommand>
{
    public UploadSupportingDocValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("File is required.");
        RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("File cannot be empty.");
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}
