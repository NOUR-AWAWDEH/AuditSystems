using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetByUserId;

public sealed record GetControlDesignsByUserIdQueryResponse(IReadOnlyList<ControlDesignModel> ControlDesignModels);
