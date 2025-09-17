using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

public interface ISupportingDocSearch
{
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<SupportingDocModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey);
}