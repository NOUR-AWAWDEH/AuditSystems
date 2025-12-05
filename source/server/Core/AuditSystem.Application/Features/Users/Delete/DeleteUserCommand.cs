using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Users.Delete;

public sealed record DeleteUserCommand(
    Guid Id
) : UserAwareCommand<Result>;
