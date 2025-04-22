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
        var totalCount = query.Count();

        if (totalCount == 0)
        {
            var emptyResult = new PaginationResult<TModel>
            {
                Items = new List<TModel>(),
                TotalCount = totalCount,
            };

            return emptyResult;
        }

        query = query.Skip((page.Number - 1) * page.Size)
            .Take(page.Size);

        var models = await repository.GetByQueryAsync(query);
        var result = new PaginationResult<TModel>
        {
            Items = models,
            TotalCount = totalCount,
        };

        return result;
    }
}