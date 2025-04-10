using FluentValidation;

namespace AuditSystem.Application.Features.Tasks.Create;

public sealed class CreateTaskManagementValidator : AbstractValidator<CreateTaskManagementCommand>
{
    public CreateTaskManagementValidator()
    {
        RuleFor(x => x.Requirement)
            .NotEmpty()
            .WithMessage("Requirement is required.")
            .MinimumLength(2)
            .WithMessage("Requirement must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Requirement must not exceed 200 characters.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due Date is required.")
            .Must(BeAValidDate)
            .WithMessage("Due Date must be a valid date.");

        RuleFor(x => x.JobName)
            .NotEmpty()
            .WithMessage("Job Name is required.")
            .MinimumLength(2)
            .WithMessage("Job Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Job Name must not exceed 200 characters.");

        RuleFor(x => x.Assignee)
            .NotEmpty()
            .WithMessage("Assignee is required.")
            .MinimumLength(2)
            .WithMessage("Assignee must be at least 2 characters long.")
            .MaximumLength(500)
            .WithMessage("Assignee must not exceed 500 characters.");

        RuleFor(x => x.AssignedById)
            .NotEmpty()
            .WithMessage("Assigned By Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Assigned By Id must be a valid GUID.");

    }

    private bool BeAValidDate(DateOnly date)
    {
        return date != default && date <= DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
