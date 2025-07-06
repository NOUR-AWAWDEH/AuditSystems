using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get;

public sealed record class GetAuditProcessByIdQueryResponse(AuditProcessModel AuditProcessModel);