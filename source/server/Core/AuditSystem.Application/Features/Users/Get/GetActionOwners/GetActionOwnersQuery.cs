using MediatR;
using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Users.Get.GetActionOwners;

public sealed record GetActionOwnersQuery : UserAwareCommand<Result<GetActionOwnersQueryResponse>>;
