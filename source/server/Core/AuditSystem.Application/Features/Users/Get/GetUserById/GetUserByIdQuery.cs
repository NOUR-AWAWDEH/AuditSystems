using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Users.Get.GetUserById;

public sealed record GetUserByIdQuery(
    Guid Id
) : UserAwareCommand<Result<GetUserByIdQueryResponse>>;
