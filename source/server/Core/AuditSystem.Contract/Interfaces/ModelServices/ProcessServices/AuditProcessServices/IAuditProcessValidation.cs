using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;


public interface IAuditProcessValidation
{
    Task<AuditProcess> ValidateAuditProcessOwnerShipAsync(Guid auditProcessId, Guid userId);
    Task<SubDepartment> ValidateSubDepartmentOwnerShipAsync(Guid subDepartmentId, Guid userId);
}