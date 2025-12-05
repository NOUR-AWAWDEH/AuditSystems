using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Users.Get.GetUsersByAdminId;

public sealed record GetUsersByAdminIdQuery : UserAwareCommand<Result<GetUsersByAdminIdQueryResponse>>;
