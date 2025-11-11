using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;

public sealed record GetSupportingDocsByUserIdQuery : UserAwareQuery<Result<GetSupportingDocsByUserIdQueryResponse>>;