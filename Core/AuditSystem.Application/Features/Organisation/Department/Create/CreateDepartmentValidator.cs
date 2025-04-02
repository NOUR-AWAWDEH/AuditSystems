using FluentValidation;

namespace AuditSystem.Application.Features.Organisation.Department.Create;

public sealed class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentValidator() 
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department Name is required.")
            .MaximumLength(200)
            .WithMessage("Department Name must not exceed 200 characters.");
        
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage("Company Id is required.");
    }
}
