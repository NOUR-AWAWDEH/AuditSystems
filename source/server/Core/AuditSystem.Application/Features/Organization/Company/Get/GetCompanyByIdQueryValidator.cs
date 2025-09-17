using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Company.Get;

public sealed class GetCompanyByIdQueryValidator : AbstractValidator<GetCompanyByIdQuery>
{
    public GetCompanyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Company Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Company Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
