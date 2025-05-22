using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

public interface ISupportingDocService
{
    public Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel);
    Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id);
    public Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel);
    public Task DeleteSupportingDocAsync(Guid supportingDocId);
    Task<List<SupportingDocModel>> GetSupportingDocsByUserIdAsync(Guid userId);
    public Task<PaginationResult<SupportingDocModel>> SearchAsync(string? name, Page page);
}
