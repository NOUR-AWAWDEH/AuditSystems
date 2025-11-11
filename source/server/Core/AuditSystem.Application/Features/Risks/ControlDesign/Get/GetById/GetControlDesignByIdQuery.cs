using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetById;

public sealed record class GetControlDesignByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetControlDesignByIdQueryResponse>>;