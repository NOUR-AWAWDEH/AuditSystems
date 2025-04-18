using FluentValidation;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Update;

public class UpdateJobSchedulingValidator : AbstractValidator<UpdateJobSchedulingCommand>
{
    public UpdateJobSchedulingValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Id must not be empty.");
       
        RuleFor(x => x.AuditableUnit)
            .NotEmpty()
            .WithMessage("AuditableUnit is required.")
            .MaximumLength(100)
            .WithMessage("AuditableUnit must not exceed 100 characters.");

        RuleFor(x => x.AuditYear)
            .NotEmpty()
            .WithMessage("AuditYear is required.")
            .Must(IsValidDate)
            .WithMessage("AuditYear must be a valid date.");

        RuleFor(x => x.PlannedStartDate)
            .NotEmpty()
            .WithMessage("PlannedStartDate is required.")
            .Must(IsValidDate)
            .WithMessage("PlannedStartDate must be a valid date.");


        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .MaximumLength(50)
            .WithMessage("Status must not exceed 50 characters.");
    }

    private static bool IsValidDate(DateOnly date)
    {
        try
        {
            var _ = new DateOnly(date.Year, date.Month, date.Day);
            return true;
        }
        catch
        {
            return false;
        }
    }
}