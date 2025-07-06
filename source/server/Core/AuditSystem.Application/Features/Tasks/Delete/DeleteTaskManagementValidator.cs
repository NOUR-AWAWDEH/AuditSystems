namespace AuditSystem.Application.Features.Tasks.Delete;
using FluentValidation;

public sealed class DeleteTaskManagementValidator : AbstractValidator<DeleteTaskManagementCommand>
{
    public DeleteTaskManagementValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Task Id is required")
           .Must(IsValidGuid)
           .WithMessage("Invalid Task Id format");
    }

    private bool IsValidGuid(Guid id)
    {
        return id!= Guid.Empty;
    }
}
