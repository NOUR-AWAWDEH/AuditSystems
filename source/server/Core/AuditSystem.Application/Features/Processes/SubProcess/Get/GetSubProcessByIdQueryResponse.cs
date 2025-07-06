using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get;

public sealed record class GetSubProcessByIdQueryResponse(SubProcessModel SubProcessModel);