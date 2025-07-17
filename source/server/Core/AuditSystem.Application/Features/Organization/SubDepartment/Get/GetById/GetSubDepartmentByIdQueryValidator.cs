using FluentValidation;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get.GetById;


public sealed class GetSubDepartmentByIdQueryValidator : AbstractValidator<GetSubDepartmentByIdQuery>
{
    public GetSubDepartmentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sub Department Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Sub Department Id must be a valid Guid");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
