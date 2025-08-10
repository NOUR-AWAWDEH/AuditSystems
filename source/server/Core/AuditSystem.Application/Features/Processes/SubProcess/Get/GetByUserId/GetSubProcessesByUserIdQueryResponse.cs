using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get.GetByUserId;

public sealed record GetSubProcessesByUserIdQueryResponse(ICollection<SubProcessModel> SubProcessModels);