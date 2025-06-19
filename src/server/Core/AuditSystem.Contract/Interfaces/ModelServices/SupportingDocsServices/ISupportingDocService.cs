using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

public interface ISupportingDocService
{
    Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel);
    Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id);
    Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel);
    Task DeleteSupportingDocAsync(Guid supportingDocId);
    Task<List<SupportingDocModel>> GetSupportingDocsByUserIdAsync(Guid userId);
    Task<PaginationResult<SupportingDocModel>> SearchAsync(string? name, Page page);
}
