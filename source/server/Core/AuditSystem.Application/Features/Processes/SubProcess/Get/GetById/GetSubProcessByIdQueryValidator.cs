using FluentValidation;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get.GetById;

public sealed class GetSubProcessByIdQueryValidator : AbstractValidator<GetSubProcessByIdQuery>
{
    public GetSubProcessByIdQueryValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .WithMessage("Sub Process Id is required")
           .Must(BeAValidGuid)
          .WithMessage("Sub Process Id must be a valid Guid"); 
          
          RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("User Id must be a valid GUID.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
