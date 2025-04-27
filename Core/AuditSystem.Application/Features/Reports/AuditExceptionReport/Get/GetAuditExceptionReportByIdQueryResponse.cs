
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Get;

public sealed record class GetAuditExceptionReportByIdQueryResponse(AuditExceptionReportModel AuditExceptionReportModel);