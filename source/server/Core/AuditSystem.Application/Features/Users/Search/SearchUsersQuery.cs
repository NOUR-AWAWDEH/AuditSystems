using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Application.Features.Users.Search;

public sealed record SearchUsersQuery(
    string? SearchTerm
) : PaginationRequest<UserModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
