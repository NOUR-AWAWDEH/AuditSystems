using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Jobs;

internal sealed class JobPrioritizationValidator<T> : PropertyValidator<T, string?>, IJobPrioritizationValidator
{
    public override string Name => "JobPrioritizationValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Job prioritization '{PropertyValue}' is not valid.";
    
    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface IJobPrioritizationValidator : IPropertyValidator { }
