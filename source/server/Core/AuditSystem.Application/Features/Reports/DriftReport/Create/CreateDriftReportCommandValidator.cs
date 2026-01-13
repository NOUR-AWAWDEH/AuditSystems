using AuditSystem.Application.Features.Reports.DriftReport.Create;
using FluentValidation;

public sealed class CreateDriftReportCommandValidator : AbstractValidator<CreateDriftReportCommand>
{
    public CreateDriftReportCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty().WithMessage("CompanyId is required.");
        RuleFor(x => x.LocationId).NotEmpty().WithMessage("LocationId is required.");
        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId is required.");
        RuleFor(x => x.SubDepartmentId).NotEmpty().WithMessage("SubDepartmentId is required.");
        RuleFor(x => x.ObjectiveId).NotEmpty().WithMessage("ObjectiveId is required.");
        RuleFor(x => x.AuditProgramId).NotEmpty().WithMessage("AuditProgramId is required.");
        RuleFor(x => x.ActionPlan).NotEmpty().MaximumLength(1000).WithMessage("ActionPlan is required and should be less than 1000 characters.");
        RuleFor(x => x.ActionDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("ActionDate cannot be in the future.");
    }
}
