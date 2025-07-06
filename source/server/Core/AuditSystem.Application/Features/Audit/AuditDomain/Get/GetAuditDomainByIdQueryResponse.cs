using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Get;

public sealed record class GetAuditDomainByIdQueryResponse(AuditDomainModel AuditDomainModel);
