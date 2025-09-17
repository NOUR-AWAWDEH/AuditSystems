using FluentValidation;

namespace AuditSystem.Application.Features.DownloadFile.Get;

public class GetFileByURLQueryValidator : AbstractValidator<GetFileByURLQuery>
{
    public GetFileByURLQueryValidator()
    {
        RuleFor(x => x.URL)
            .NotEmpty().WithMessage("URL is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Invalid URL format.");
    }
}