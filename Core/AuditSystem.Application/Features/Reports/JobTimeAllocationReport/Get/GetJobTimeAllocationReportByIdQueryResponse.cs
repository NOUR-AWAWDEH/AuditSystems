

using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Get;

public sealed record class GetJobTimeAllocationReportByIdQueryResponse(JobTimeAllocationReportModel JobTimeAllocationReportModel);