using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Get.GetById;

public sealed class GetDepartmentByIdQueryValidator : AbstractValidator<GetDepartmentByIdQuery>
{
    public GetDepartmentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Department Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Department Id must be a valid Guid");
        
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
