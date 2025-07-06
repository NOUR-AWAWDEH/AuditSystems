using FluentValidation;

namespace AuditSystem.Application.Features.Common.Rating.Delete;

public sealed class DeleteRatingValidator : AbstractValidator<DeleteRatingCommand>
{
    public DeleteRatingValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Rating Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Rating Id cannot be empty.");
    }

    private bool IsValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}