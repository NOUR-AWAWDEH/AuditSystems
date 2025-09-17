using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;

public sealed class UpdateRiskControlMatrixValidator : AbstractValidator<UpdateRiskControlMatrixCommand>
{
    public UpdateRiskControlMatrixValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(BeAValidGuid)
            .WithMessage("Id must be a valid Guid.");

        RuleFor(x => x.SubProcessId)
            .NotEmpty()
            .WithMessage("SubProcessId is required.")
            .Must(BeAValidGuid)
            .WithMessage("SubProcessId must be a valid Guid.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .Must(BeAValidGuid)
            .WithMessage("UserId must be a valid Guid.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}