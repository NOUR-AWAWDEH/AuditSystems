using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Get;

public sealed record class GetAuditPlanSummaryByIdQueryResponse(AuditPlanSummaryModel AuditPlanSummaryModel);