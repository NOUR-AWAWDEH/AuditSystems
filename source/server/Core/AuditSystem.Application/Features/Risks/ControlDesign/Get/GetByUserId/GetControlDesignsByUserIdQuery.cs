using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetByUserId;

public sealed record class GetControlDesignsByUserIdQuery : UserAwareQuery<Result<GetControlDesignsByUserIdQueryResponse>>;