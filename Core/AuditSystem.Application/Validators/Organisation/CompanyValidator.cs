using FluentValidation;
using FluentValidation.Validators;

namespace AuditSystem.Application.Validators.Organisation;

internal sealed class CompanyValidator<T> : PropertyValidator<T, string?>, ICompanyValidator
{
    public override string Name => "CompanyValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Company '{PropertyValue}' is not valid.";

    public override bool IsValid(ValidationContext<T> context, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Guid.TryParse(value, out _);
    }
}

public interface ICompanyValidator : IPropertyValidator { }
