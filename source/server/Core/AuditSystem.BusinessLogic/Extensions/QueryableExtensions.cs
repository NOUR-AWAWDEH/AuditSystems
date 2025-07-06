using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.BusinessLogic.Extensions;

public static class QueryableExtensions
{
    public static async ValueTask<PaginationResult<TModel>> PaginateAsync<TId, TEntity, TModel>(
        this IQueryable<TModel> query,
        Page page,
        IRepository<TId, TEntity> repository)
        where TEntity : Entity<TId>
        where TId : IComparable<TId>
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(page);
        ArgumentNullException.ThrowIfNull(repository);

        if (page.Size <= 0)
            throw new ArgumentOutOfRangeException(nameof(page.Size), "Page size must be greater than zero.");

        if (page.Number < 1)
            throw new ArgumentOutOfRangeException(nameof(page.Number), "Page number must be greater than or equal to one.");

        var totalCount = query.Count();

        if (totalCount == 0)
        {
            return new PaginationResult<TModel>
            {
                Items = Array.Empty<TModel>(),
                TotalCount = 0,
                PageNumber = page.Number,
                PageSize = page.Size
            };
        }

        var pagedQuery = query
            .Skip((page.Number - 1) * page.Size)
            .Take(page.Size);

        var models = await repository.GetByQueryAsync(pagedQuery);

        return new PaginationResult<TModel>
        {
            Items = models,
            TotalCount = totalCount,
            PageNumber = page.Number,
            PageSize = page.Size
        };
    }
}