using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.JobTitle.Get.GetByUserId;

internal sealed class GetJobTitlesByUserIdQueryHandler(
    IJobTitleService jobTitleService,
    ILogger<GetJobTitlesByUserIdQueryHandler> logger ) : 
    IRequestHandler<GetJobTitlesByUserIdQuery, Result<GetJobTitlesByUserIdQueryResponse>>
{
    public async Task<Result<GetJobTitlesByUserIdQueryResponse>> Handle(
        GetJobTitlesByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var jobTitles = await jobTitleService.GetJobTitlesByUserIdAsync(request.UserId);
        logger.LogDebug("Job titles retrieved successfully for user {UserId}", request.UserId);
        return Result<GetJobTitlesByUserIdQueryResponse>.Success(
            new(jobTitles),
            "Job titles retrieved successfully");
    }
}
