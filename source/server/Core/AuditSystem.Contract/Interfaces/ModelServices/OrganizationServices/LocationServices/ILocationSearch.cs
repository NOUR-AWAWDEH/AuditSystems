using System.Linq.Expressions;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;

public interface ILocationSearch
{
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<LocationModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Page page, string cacheKey, string countCacheKey);
}