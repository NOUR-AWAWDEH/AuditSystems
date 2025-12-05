using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Application.Features.Users.Get.GetUserById;

public sealed record GetUserByIdQueryResponse(UserModel User);
