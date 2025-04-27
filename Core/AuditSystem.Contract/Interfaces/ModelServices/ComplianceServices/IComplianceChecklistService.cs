using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;

public interface IComplianceChecklistService
{
    public Task<Guid> CreateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel);
    public Task UpdateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel);
    public Task DeleteComplianceChecklistAsync(Guid complianceChecklistId);
    public Task<ComplianceChecklistModel> GetComplianceChecklistByIdAsync(Guid id);
    public Task<PaginationResult<ComplianceChecklistModel>> SearchAsync(string? name, Page page);
}
