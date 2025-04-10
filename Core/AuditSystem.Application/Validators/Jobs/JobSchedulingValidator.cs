using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Jobs;

internal sealed class JobSchedulingValidator<T> : PropertyValidator<T, string?>, IJobSchedulingValidator
{
    public override string Name => "JobSchedulingValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Job scheduling '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IJobSchedulingValidator : IPropertyValidator { }
