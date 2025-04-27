using FluentValidation;

namespace AuditSystem.Application.Features.SupportingDocs.Update;
public sealed class UpdateSupportingDocValidator : AbstractValidator<UpdateSupportingDocCommand>
{
    public UpdateSupportingDocValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required.")
            .MinimumLength(2)
            .WithMessage("FileName must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("FileName must not exceed 100 characters.");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("FileSize must be greater than 0.");

        RuleFor(x => x.URL)
            .NotEmpty()
            .WithMessage("URL is required.")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("URL must be a valid absolute URI.");
    }
}