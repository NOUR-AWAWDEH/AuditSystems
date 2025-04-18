using AuditSystem.Domain.Entities.Common;
using System.Linq.Expressions;

namespace AuditSystem.Contract.Interfaces.Repositories;

public interface IRepository<TId, TEntity>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task CreateRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> GetByIdAsync(TId id);
    Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object?>>[]? ignoreProperties = null);
    Task DeleteAsync(TId id);
    ValueTask DeleteRangeAsync(ICollection<TEntity> entities);
    IQueryable<TEntity> GetAll();
    Task<ICollection<TResponse>> GetByQueryAsync<TResponse>(IQueryable<TResponse> query);
}