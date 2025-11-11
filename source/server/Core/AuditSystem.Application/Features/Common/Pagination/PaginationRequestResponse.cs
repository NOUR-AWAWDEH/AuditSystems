namespace AuditSystem.Application.Features.Common.Pagination;

public record PaginationRequestResponse<TModel>(IReadOnlyCollection<TModel> Items, int TotalCount);
