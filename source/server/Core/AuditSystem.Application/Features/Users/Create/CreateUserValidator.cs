using FluentValidation;
using System.Text.RegularExpressions;

namespace AuditSystem.Application.Features.Users.Create;

public sealed partial class CreateUserValidator : AbstractValidator<CreateUserCommand>
{

    [GeneratedRegex("^[a-zA-Z0-9_.-]+$")]
    private static partial Regex UsernameRegex();

    public CreateUserValidator()
    {
        RuleFor(x => x.UserName)
             .Cascade(CascadeMode.Stop)
             .NotEmpty().WithMessage("Username is required.")
             .Must(NotStartWithSpace).WithMessage("Username cannot start with a space.")
             .Must(NotEndWithSpace).WithMessage("Username cannot end with a space.")
             .Length(4, 50).WithMessage("Username must be between 4 and 50 characters.")
             .Must(BeValidUsername)
             .WithMessage("Username can only contain letters, numbers, underscores, dots, and hyphens.");


        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("First name is required.")
            .Must(NotStartWithSpace).WithMessage("First name cannot start with a space.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Last name is required.")
            .Must(NotStartWithSpace).WithMessage("Last name cannot start with a space.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role is required.");

        RuleFor(x => x.CompanyId)
            .NotEqual(Guid.Empty).WithMessage("Company ID is required.");

        RuleFor(x => x.JobTitleId)
            .Must(BeValidNullableGuid)
            .WithMessage("Invalid job title ID.");

        RuleFor(x => x.SubDepartmentId)
            .Must(BeValidNullableGuid)
            .WithMessage("Invalid sub-department ID.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("User ID is required.");
    }


    private static bool NotStartWithSpace(string value)
        => !string.IsNullOrEmpty(value) && !value.StartsWith(" ");

    private static bool NotEndWithSpace(string value)
        => !string.IsNullOrEmpty(value) && !value.EndsWith(" ");

    private static bool BeValidUsername(string username)
        => UsernameRegex().IsMatch(username);

    private static bool BeValidNullableGuid(Guid? id)
        => id is null || id != Guid.Empty;
}