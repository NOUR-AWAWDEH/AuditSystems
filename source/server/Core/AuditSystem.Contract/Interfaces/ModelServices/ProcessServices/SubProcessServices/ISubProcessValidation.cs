using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;

public interface ISubProcessValidation
{
    Task<SubProcess> ValidateSubProcessOwnershipAsync(Guid subProcessId, Guid userId);
    Task<AuditProcess> ValidateAuditProcessOwnershipAsync(Guid auditProcessId, Guid userId);
}