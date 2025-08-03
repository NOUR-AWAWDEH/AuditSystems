using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;

public interface ISubProcessValidation
{
    Task<SubProcess> ValidateSubProcessOwnerShipAsync(Guid subProcessId, Guid userId);
    Task<AuditProcess> ValidateAuditProcessOwnerShipAsync(Guid auditProcessId, Guid userId);
}