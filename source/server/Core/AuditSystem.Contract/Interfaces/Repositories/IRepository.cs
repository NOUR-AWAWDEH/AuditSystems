using AuditSystem.Domain.Entities.Common;
using System.Linq.Expressions;

namespace AuditSystem.Contract.Interfaces.Repositories;

public interface IRepository<TId, TEntity>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task CreateRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> GetByIdAsync(TId id, Expression<Func<TEntity, object?>>[]? include = null);
    Task<TEntity?> GetByIdAsync(TId id);
    // Add generic method for cross-entity queries
    Task<TOtherEntity?> GetByIdAsync<TOtherEntity>(object id) where TOtherEntity : class;
    // Add generic method for cross-entity queries with includes
    Task<TOtherEntity?> GetByIdAsync<TOtherEntity>(object id, Expression<Func<TOtherEntity, object?>>[]? include = null) where TOtherEntity : class;
    // Enhanced method with include support
    Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object?>>[]? ignoreProperties = null);
    Task DeleteAsync(TId id);
    ValueTask DeleteRangeAsync(ICollection<TEntity> entities);
    IQueryable<TEntity> GetAll();
    Task<ICollection<TResponse>> GetByQueryAsync<TResponse>(IQueryable<TResponse> query);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<ICollection<TResponse>> ProjectToAsync<TResponse>(IQueryable<TEntity> query);
    IQueryable<TEntity> Query();
    Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
}