using FluentValidation;

namespace AuditSystem.Application.Features.Organization.JobTitle.Get.GetById;

public sealed class GetJobTitleByIdQueryValidator : AbstractValidator<GetJobTitleByIdQuery>
{
    public GetJobTitleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Job title ID is required.")
            .Must(IsValidGuid)
            .WithMessage("Job title ID must be a valid GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.")
            .Must(IsValidGuid)
            .WithMessage("User Id must be a valid GUID.");
    }

    private static bool IsValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}