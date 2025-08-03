using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get.GetById;

public sealed record GetAuditProcessByIdQueryResponse(AuditProcessModel AuditProcessModel);