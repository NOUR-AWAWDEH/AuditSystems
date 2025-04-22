using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get;

public sealed class GetSubProcessByIdQueryValidator : AbstractValidator<GetSubProcessByIdQuery>
{
    public GetSubProcessByIdQueryValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Sub Process Id is required")
           .Must(BeAValidGuid)
          .WithMessage("Sub Process Id must be a valid Guid");  
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
