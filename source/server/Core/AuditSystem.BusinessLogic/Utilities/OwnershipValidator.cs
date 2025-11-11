using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Utilities;

public static class OwnershipValidator
{
    public static async Task<TEntity> ValidateOwnershipAsync<TEntity, TKey>(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>> entityFilter,
        Expression<Func<TEntity, Guid>> ownerIdSelector,
        Guid userId,
        string notFoundMessage,
        string unauthorizedMessage)
        where TEntity : class
    {
        // First check only the owner ID using proper EF translation
        var ownerId = await query
            .Where(entityFilter)
            .AsNoTracking()
            .Select(ownerIdSelector) // This will be translated to SQL
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty)
        {
            throw new KeyNotFoundException(notFoundMessage);
        }

        if (ownerId != userId)
        {
            throw new UnauthorizedAccessException(unauthorizedMessage);
        }

        // Now fetch the actual entity (it exists and user owns it)
        var entity = await query
            .Where(entityFilter)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return entity!; // We know it exists from the previous check
    }
}