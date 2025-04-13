using FluentValidation;

namespace AuditSystem.Application.Features.Tasks.Update;

public sealed class UpdateTaskManagementValidator : AbstractValidator<UpdateTaskManagementCommand>
{
    public UpdateTaskManagementValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.Requirement)
            .NotEmpty()
            .WithMessage("Requirement is required.")
            .MinimumLength(2)
            .WithMessage("Requirement must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Requirement must not exceed 100 characters.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("DueDate is required.")
            .Must(date => IsValidDate(date))
            .WithMessage("DueDate must be a valid date.");

        RuleFor(x => x.JobName)
            .NotEmpty()
            .WithMessage("JobName is required.")
            .MinimumLength(2)
            .WithMessage("JobName must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("JobName must not exceed 100 characters.");

        RuleFor(x => x.Assignee)
            .NotEmpty()
            .WithMessage("Assignee is required.")
            .MinimumLength(2)
            .WithMessage("Assignee must be at least 2 characters long.")
            .MaximumLength(100)
            .WithMessage("Assignee must not exceed 100 characters.");

        RuleFor(x => x.AssignedById)
            .NotEmpty()
            .WithMessage("AssignedById is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("AssignedById must not be empty.");
    }
    public bool IsValidDate(DateOnly date)
    {
        return date != default(DateOnly);
    }
}