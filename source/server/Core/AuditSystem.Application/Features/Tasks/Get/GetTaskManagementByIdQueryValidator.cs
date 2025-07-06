using FluentValidation;

namespace AuditSystem.Application.Features.Tasks.Get;

public sealed class GetTaskManagementByIdQueryValidator : AbstractValidator<GetTaskManagementByIdQuery>
{
    public GetTaskManagementByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Task Management Id is required")
            .Must(BeAValidGuid)
            .WithMessage("Task Management Id must be a valid Guid"); 
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }    
}
