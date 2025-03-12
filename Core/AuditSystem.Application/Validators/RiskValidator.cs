using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators
{
    internal sealed class RiskValidator<T> : PropertyValidator<T, string?>, IRiskValidator
    {
        public override string Name => "RiskValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "Risk property '{PropertyValue}' is not valid.";

        public override bool IsValid(ValidationContext<T> context, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return Guid.TryParse(value, out _);
        }
    }

    public interface IRiskValidator : IPropertyValidator { }
}
