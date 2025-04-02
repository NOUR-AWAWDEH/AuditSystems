using AuditSystem.Contract.Models.Compliance;

namespace AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;

public interface IComplianceChecklistService
{
    public Task<Guid> CreateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel);
}
