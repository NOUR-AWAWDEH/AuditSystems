using FluentValidation;

namespace AuditSystem.Application.Base.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected BaseValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }
}