using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Get;

public sealed record class GetReportingFollowUpByIdQueryResponse(ReportingFollowUpModel ReportingFollowUpModel);