using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Get;

public sealed record class GetAuditJobByIdQueryResponse(AuditJobModel AuditJobModel);