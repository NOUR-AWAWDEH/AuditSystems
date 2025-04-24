using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Contract.Interfaces.Cache;

public interface ICacheRepositoryHandler<TId, TEntity>
        where TEntity : Entity<TId>
        where TId : IComparable<TId>
    {
        Task<PaginationResult<TModel>> GetFromCacheAsync<TModel>(string cacheKey, string countCacheKey, Page page);
        Task<PaginationResult<TModel>> GetFromRepositoryAsync<TModel>(string cacheKey, string countCacheKey, Page page, string? name);
    }