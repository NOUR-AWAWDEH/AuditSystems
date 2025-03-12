using AuditSystem.Domain.Entities.Common;
using System.Linq.Expressions;

namespace AuditSystem.BusinessLogic.Repositories;

public interface IRepository<TId, TEntity>
    where TEntity : Entity<TId>
    where TId : IComparable<TId>
{
    public Task<TEntity> CreateAsync(TEntity entity);
    public Task CreateRangeAsync(IEnumerable<TEntity> entity);
    public Task<TEntity?> GetAsync(TId id);
    public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object?>>[]? ignoreProperties = null);
    public Task DeleteAsync(TId id);
    public ValueTask DeleteRangeAsync(ICollection<TEntity> entities);
    public IQueryable<TEntity> GetAll();
    Task<ICollection<TResponse>> GetByQueryAsync<TResponse>(IQueryable<TResponse> query);
}
