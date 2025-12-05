using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Application.Features.Users.Get.GetUsersByAdminId;

public sealed record GetUsersByAdminIdQueryResponse(IReadOnlyList<UserModel> Users);
