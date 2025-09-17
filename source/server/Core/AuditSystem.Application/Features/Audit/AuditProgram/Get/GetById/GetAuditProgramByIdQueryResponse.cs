using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Get.GetById;

public sealed record class GetAuditProgramByIdQueryResponse(AuditProgramModel AuditProgramModel);
