using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Get;

public sealed record class GetAuditUniverseByIdQueryResponse(AuditUniverseModel AuditUniverseModel);