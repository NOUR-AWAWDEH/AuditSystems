using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Get;

internal sealed class GetReportingFollowUpByIdQueryHandler( 
    IReportingFollowUpService reportingFollowUpService) : 
    IRequestHandler<GetReportingFollowUpByIdQuery, Result<GetReportingFollowUpByIdQueryResponse>>
{
    public async Task<Result<GetReportingFollowUpByIdQueryResponse>> Handle(GetReportingFollowUpByIdQuery request, CancellationToken cancellationToken) 
    {
        var result = await reportingFollowUpService.GetReportingFollowUpByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetReportingFollowUpByIdQueryResponse>.Success(new GetReportingFollowUpByIdQueryResponse(result));
    }
}
