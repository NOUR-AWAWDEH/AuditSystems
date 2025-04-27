using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubLocation.Delete;

public sealed class DeleteSubLocationValidator : AbstractValidator<DeleteSubLocationCommand>
{
    public DeleteSubLocationValidator() 
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sub Location Id field is required")
            .Must(IsValidGuid)
            .WithMessage("Sub Location Id must be a valid Id");

    }

    private bool IsValidGuid(Guid id) 
    {
        return id != Guid.Empty;
    }
}
