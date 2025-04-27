using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Get;

internal sealed class GetJobTimeAllocationReportByIdQueryHandler(
    IJobTimeAllocationReportService jobTimeAllocationReportService) : 
    IRequestHandler<GetJobTimeAllocationReportByIdQuery, Result<GetJobTimeAllocationReportByIdQueryResponse>>
{
    public async Task<Result<GetJobTimeAllocationReportByIdQueryResponse>> Handle(GetJobTimeAllocationReportByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await jobTimeAllocationReportService.GetJobTimeAllocationReportByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetJobTimeAllocationReportByIdQueryResponse>.Success(new GetJobTimeAllocationReportByIdQueryResponse(result));
    }
}
