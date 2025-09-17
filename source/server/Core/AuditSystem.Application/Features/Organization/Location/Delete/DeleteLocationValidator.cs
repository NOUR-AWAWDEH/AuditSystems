using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Location.Delete;

public sealed class DeleteLocationValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Location Id is required.")
            .Must(IsAValidGuid)
            .WithMessage("Location Id is not valid.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool IsAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
