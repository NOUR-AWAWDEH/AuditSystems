using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Get;

internal sealed class GetJobSchedulingByIdQueryHandler(
    IJobSchedulingService jobSchedulingService) :
    IRequestHandler<GetJobSchedulingByIdQuery, Result<GetJobSchedulingByIdQueryResponse>> 
{
    public async Task<Result<GetJobSchedulingByIdQueryResponse>> Handle( GetJobSchedulingByIdQuery request,CancellationToken cancellationToken)
    {
        var result = await jobSchedulingService.GetJobSchedulingByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetJobSchedulingByIdQueryResponse>.Success(new GetJobSchedulingByIdQueryResponse(result));
    }
}
