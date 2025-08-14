using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.Objective.Get.GetByUserId;

public sealed record GetObjectivesByUserIdQueryResponse(ICollection<ObjectiveModel> ObjectiveModels);
