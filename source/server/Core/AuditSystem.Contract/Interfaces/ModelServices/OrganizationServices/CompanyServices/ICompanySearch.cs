using System.Linq.Expressions;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.CompanyServices;

public interface ICompanySearch
{
    Expression<Func<Company, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<CompanyModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey);
}
