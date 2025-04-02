using FluentValidation;

namespace AuditSystem.Application.Features.Tasks.Create;

public sealed class CreateTaskManagementValidator : AbstractValidator<CreateTaskManagementCommand>
{
    public CreateTaskManagementValidator() 
    {
        RuleFor(x => x.Requirement)
            .NotEmpty()
            .WithMessage("Requirement is required.")
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
            .MaximumLength(200)
            .WithMessage("Job Name must not exceed 200 characters.");

        RuleFor(x => x.Assignee)
            .NotEmpty()
            .WithMessage("Assignee is required.")
            .MaximumLength(500)
            .WithMessage("Assignee must not exceed 500 characters.");

        RuleFor(x => x.AssignedById)
            .NotEmpty()
            .WithMessage("Assigned By Id is required.");
    }

    private bool BeAValidDate(DateOnly date)
    {
        return date != default;
    }
}
