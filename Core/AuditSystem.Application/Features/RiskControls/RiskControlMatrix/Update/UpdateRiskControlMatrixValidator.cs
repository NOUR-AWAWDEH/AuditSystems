using FluentValidation;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;

public sealed class UpdateRiskControlMatrixValidator : AbstractValidator<UpdateRiskControlMatrixCommand>
{
    public UpdateRiskControlMatrixValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must not be empty.");

        RuleFor(x => x.SubProcessId)
            .NotEmpty()
            .WithMessage("SubProcessId is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("SubProcessId must not be empty.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}