using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuditSystem.DataAccess.Repositories;

internal sealed class EntityRepository<TId, TEntity>(ApplicationDbContext context) : IRepository<TId, TEntity>
    where TId : IComparable<TId>
    where TEntity : Entity<TId>
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var result = await _entities.AddAsync(entity);
        await context.SaveChangesAsync();
        context.Entry(entity).State = EntityState.Detached;
        return result.Entity;
    }

    public async Task CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        await _entities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
        entities.ToList().ForEach(e => context.Entry(e).State = EntityState.Detached);
    }

    public async Task<TEntity?> GetAsync(TId id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, object?>>[]? ignoreProperties = null)
    {
        _entities.Update(entity);
        
        if (ignoreProperties != null)
        {
            foreach (var property in ignoreProperties)
            {
                context.Entry(entity).Property(property).IsModified = false;
            }
        }
        
        await context.SaveChangesAsync();
        context.Entry(entity).State = EntityState.Detached;
    }

    public async Task DeleteAsync(TId id)
    {
        var entity = await GetAsync(id);
        if (entity != null)
        {
            _entities.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async ValueTask DeleteRangeAsync(ICollection<TEntity> entities)
    {
        _entities.RemoveRange(entities);
        await context.SaveChangesAsync();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _entities.AsNoTracking();
    }

    public async Task<ICollection<TResponse>> GetByQueryAsync<TResponse>(IQueryable<TResponse> query)
    {
        return await query.ToListAsync();
    }
}