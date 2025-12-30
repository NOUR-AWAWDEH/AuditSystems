using AuditSystem.Contract.Interfaces;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Contract.Models.Reports.DriftReport;
   
public class GenerateDriftReportRequest  : IUserAwareRequest
{
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid CompanyId { get; set; } = Guid.Empty;
    public Guid LocationId { get; set; } = Guid.Empty;
    public Guid DepartmentId { get; set; } = Guid.Empty;
    public Guid SubDepartmentId { get; set; } = Guid.Empty;
    public Guid ObjectiveId { get; set; }  = Guid.Empty;
    public Guid AuditProgramId { get; set; } = Guid.Empty;
}