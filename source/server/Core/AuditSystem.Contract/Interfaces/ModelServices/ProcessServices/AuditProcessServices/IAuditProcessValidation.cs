using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;


public interface IAuditProcessValidation
{
    Task<AuditProcess> ValidateAuditProcessOwnershipAsync(Guid auditProcessId, Guid userId);
    Task<SubDepartment> ValidateSubDepartmentOwnershipAsync(Guid subDepartmentId, Guid userId);
}