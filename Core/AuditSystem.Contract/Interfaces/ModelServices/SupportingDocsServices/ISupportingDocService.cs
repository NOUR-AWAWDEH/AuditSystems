using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

public interface ISupportingDocService
{
    public Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel);
    public Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel);
    public Task DeleteSupportingDocAsync(Guid supportingDocId);
    public Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id);
    public Task<PaginationResult<SupportingDocModel>> SearchAsync(string? name, Page page);
}
