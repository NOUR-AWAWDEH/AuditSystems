using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetByUserId;

public sealed record GetAuditProgramsByUserIdQueryResponse(IReadOnlyList<AuditProgramModel> AuditPrograms);
