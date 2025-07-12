using System.Linq.Expressions;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;


public interface ISubLocationSearch
{
    Expression<Func<SubLocation, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid? locationId = null);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? locationId, Page page);
    Task<PaginationResult<SubLocationModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey);
    Task<PaginationResult<SubLocationModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid? locationId, Page page, string cacheKey, string countCacheKey);
}