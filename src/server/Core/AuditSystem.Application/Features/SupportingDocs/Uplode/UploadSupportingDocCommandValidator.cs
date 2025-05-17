using FluentValidation;

namespace AuditSystem.Application.Features.SupportingDocs.Uplode;

public sealed class UploadSupportingDocCommandValidator : AbstractValidator<UploadSupportingDocCommand>
{
    
    public UploadSupportingDocCommandValidator()
    {
        // Validate the File property
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.");

        // Validate the FileName property
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name is required.");

        // Validate the UserId property
        RuleFor(x => x.UserId)
            .NotEmpty() // Ensures the Guid is not default (all zeros)
            .WithMessage("User ID is required and must not be empty.")
            // Although NotEmpty covers Guid.Empty, explicitly checking can be clearer sometimes
            .Must(id => id != Guid.Empty)
            .WithMessage("User ID must not be an empty GUID.");
    }
}