using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Delete;

public sealed class DeleteJobSchedulingValidator : AbstractValidator<DeleteJobSchedulingCommand>
{
    public DeleteJobSchedulingValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(IsValidGuid)
            .WithMessage("Id must be a valid GUID.");
    }

    private static bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
