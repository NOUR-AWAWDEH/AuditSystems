using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetByUserId;

public sealed record GetAuditProcessesByUserIdQueryResponse(ICollection<AuditProcessModel> AuditProcesses);