using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.ExitControl.Get.GetByUserId;

public sealed record GetExitControlsByUserIdQueryResponse(IReadOnlyList<ExitControlModel> ExitControlModels);
