using System.Linq.Expressions;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;

public interface IAppUserSearch
{
    Expression<Func<AppUser, bool>> BuildExpression(Guid userId, string? searchTerm, Guid? subDepartmentId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? subDepartmentId, Page page);
    Task<PaginationResult<UserModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid? subDepartmentId, Page page, string cacheKey, string countCacheKey);
}