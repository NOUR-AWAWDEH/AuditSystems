namespace AuditSystem.Application.Features.Common.Pagination;

public record PaginationRequestResponse<TModel>(ICollection<TModel> Items, int TotalCount);