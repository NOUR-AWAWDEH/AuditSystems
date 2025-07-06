using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Get;

public sealed record class GetAuditEngagementByIdQueryResponse(AuditEngagementModel AuditEngagement);