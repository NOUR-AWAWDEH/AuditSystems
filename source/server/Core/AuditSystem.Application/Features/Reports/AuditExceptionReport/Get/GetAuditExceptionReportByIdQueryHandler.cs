using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Get;

internal sealed class GetAuditExceptionReportByIdQueryHandler(
    IAuditExceptionReportService auditExceptionReportService) : 
    IRequestHandler<GetAuditExceptionReportByIdQuery, Result<GetAuditExceptionReportByIdQueryResponse>>
{
    public async Task<Result<GetAuditExceptionReportByIdQueryResponse>> Handle(GetAuditExceptionReportByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await auditExceptionReportService.GetAuditExceptionReportByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetAuditExceptionReportByIdQueryResponse>.Success(new GetAuditExceptionReportByIdQueryResponse(result));
    }
}