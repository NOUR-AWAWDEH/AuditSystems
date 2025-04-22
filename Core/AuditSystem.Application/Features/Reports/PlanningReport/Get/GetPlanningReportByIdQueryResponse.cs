using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Get;

public sealed record class GetPlanningReportByIdQueryResponse(PlanningReportModel PlanningReportModel);