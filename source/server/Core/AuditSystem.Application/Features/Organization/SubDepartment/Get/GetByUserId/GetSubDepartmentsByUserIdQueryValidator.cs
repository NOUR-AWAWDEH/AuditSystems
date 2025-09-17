using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get.GetByUserId;


public sealed class GetSubDepartmentsByUserIdQueryValidator : AbstractValidator<GetSubDepartmentsByUserIdQuery>
{
    public GetSubDepartmentsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
