using Ardalis.Result;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;
using Microsoft.AspNetCore.Http;

namespace AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

public interface ISupportingDocService
{
    Task<Result<Guid>> CreateSupportingDocAsync(IFormFile file, string fileName, Guid userId);
    Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel, Guid userId);
    Task DeleteSupportingDocAsync(Guid supportingDocId, Guid userId);
    Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id, Guid userId);
    Task<IReadOnlyList<SupportingDocModel>> GetSupportingDocsByUserIdAsync(Guid userId);
    Task<PaginationResult<SupportingDocModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<SupportingDocModel>> SearchAsync(string? name, Page page);
}
