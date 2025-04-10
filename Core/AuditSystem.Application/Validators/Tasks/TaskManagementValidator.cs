using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Tasks;

internal sealed class TaskManagementValidator<T> : PropertyValidator<T, string?>, ITaskManagementValidator
{
    public override string Name => "TaskManagementValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Task '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ITaskManagementValidator : IPropertyValidator { }
