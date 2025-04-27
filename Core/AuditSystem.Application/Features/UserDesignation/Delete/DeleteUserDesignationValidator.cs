namespace AuditSystem.Application.Features.UserDesignation.Delete;
using FluentValidation;

public sealed class DeleteUserDesignationValidator : AbstractValidator<DeleteUserDesignationCommand>
{
    public DeleteUserDesignationValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("User Designation Id is required")
           .Must(IsValidGuid)
           .WithMessage("Invalid User Designation Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
