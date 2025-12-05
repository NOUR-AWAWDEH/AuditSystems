using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace AuditSystem.DataAccess.Repositories;

internal sealed class EntityRepository<TId, TEntity>(ApplicationDbContext context, IMapper mapper)
    : IRepository<TId, TEntity>
    where TId : IComparable<TId>
    where TEntity : Entity<TId>
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();
    private readonly IMapper _mapper = mapper;

    public IQueryable<TEntity> Query()
    {
        return _entities.AsQueryable();
    }
    
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
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, Expression<Func<TEntity, object?>>[]? include = null)
    {
        IQueryable<TEntity> query = _entities.AsQueryable();

        if (include != null && include.Length > 0)
        {
            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }
        }
        
        return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    // Generic method for cross-entity queries
    public async Task<TOtherEntity?> GetByIdAsync<TOtherEntity>(object id) where TOtherEntity : class
    {
        return await context.Set<TOtherEntity>().FindAsync(id);
    }

    // Generic method for cross-entity queries with includes
    public async Task<TOtherEntity?> GetByIdAsync<TOtherEntity>(object id, Expression<Func<TOtherEntity, object?>>[]? include = null) where TOtherEntity : class
    {
        IQueryable<TOtherEntity> query = context.Set<TOtherEntity>().AsQueryable();

        if (include != null && include.Length > 0)
        {
            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }
        }
        
        return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
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
        var entity = await GetByIdAsync(id);
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

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entities.AsNoTracking().CountAsync(predicate);
    }

    public async Task<ICollection<TResponse>> ProjectToAsync<TResponse>(IQueryable<TEntity> query)
    {
        return await query.ProjectTo<TResponse>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        IQueryable<TEntity> query = _entities.AsNoTracking();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await context.Database.BeginTransactionAsync();
    }
}