using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Application.Features.Users.Get.GetActionOwners;

public sealed record GetActionOwnersQueryResponse(List<UserModel> ActionOwners);
