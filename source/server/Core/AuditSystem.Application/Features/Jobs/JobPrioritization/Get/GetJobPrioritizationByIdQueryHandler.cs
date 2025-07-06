using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Get;

internal sealed class GetJobPrioritizationByIdQueryHandler(
    IJobPrioritizationService jobPrioritizationService) : 
    IRequestHandler<GetJobPrioritizationByIdQuery, Result<GetJobPrioritizationByIdQueryResponse>>
{
    public async Task<Result<GetJobPrioritizationByIdQueryResponse>> Handle(GetJobPrioritizationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await jobPrioritizationService.GetJobPrioritizationByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetJobPrioritizationByIdQueryResponse>.Success(new GetJobPrioritizationByIdQueryResponse(result)); 
    }
}