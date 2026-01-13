using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.DriftReport.Create;

public sealed record class CreateDriftReportCommand(
    Guid CompanyId,
    Guid LocationId,
    Guid DepartmentId,
    Guid SubDepartmentId,
    Guid ObjectiveId,  
    Guid AuditProgramId, 
    DateTime ActionDate, 
    string ActionPlan, 
    bool IsAgreed, 
    bool IsActive
) : UserAwareCommand<Result<CreateDriftReportCommandResponse>>;