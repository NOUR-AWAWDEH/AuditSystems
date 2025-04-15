using AuditSystem.Contract.Models.Compliance;

namespace AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;

public interface IComplianceChecklistService
{
    public Task<Guid> CreateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel);
    public Task UpdateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel);
    public Task DeleteComplianceChecklistAsync(Guid complianceChecklistId);
}
