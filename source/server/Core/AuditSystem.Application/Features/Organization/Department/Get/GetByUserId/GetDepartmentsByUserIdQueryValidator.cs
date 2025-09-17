using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Get.GetByUserId;

public sealed class GetDepartmentsByUserIdQueryValidator : AbstractValidator<GetDepartmentsByUserIdQuery>
{
    public GetDepartmentsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .Must(BeAValidGuid)
            .WithMessage("User Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}