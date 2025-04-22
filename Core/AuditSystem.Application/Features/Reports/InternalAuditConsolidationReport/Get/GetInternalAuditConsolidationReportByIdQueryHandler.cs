using MediatR;
using Ardalis.Result;

using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Get;

internal sealed class GetInternalAuditConsolidationReportByIdQueryHandler(
    IInternalAuditConsolidationReportService internalAuditConsolidationReportService) : 
    IRequestHandler<GetInternalAuditConsolidationReportByIdQuery, Result<GetInternalAuditConsolidationReportByIdQueryResponse>>
{
    public async Task<Result<GetInternalAuditConsolidationReportByIdQueryResponse>> Handle(GetInternalAuditConsolidationReportByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await internalAuditConsolidationReportService.GetInternalAuditConsolidationReportByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetInternalAuditConsolidationReportByIdQueryResponse>.Success(new GetInternalAuditConsolidationReportByIdQueryResponse(result));
    }
}
