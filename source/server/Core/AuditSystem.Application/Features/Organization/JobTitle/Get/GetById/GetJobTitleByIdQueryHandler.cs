using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.JobTitle.Get.GetById;

internal sealed class GetJobTitleByIdQueryHandler(
    IJobTitleService jobTitleService,
    ILogger<GetJobTitleByIdQueryHandler> logger)
    : IRequestHandler<GetJobTitleByIdQuery, Result<GetJobTitleByIdQueryResponse>>
{
    public async Task<Result<GetJobTitleByIdQueryResponse>> Handle(GetJobTitleByIdQuery request, CancellationToken cancellationToken)
    {
        var jobTitle = await jobTitleService.GetJobTitleByIdAsync(request.Id, request.UserId);
        
        logger.LogInformation("Job title with id: {JobTitleId} has been retrieved successfully by user with id: {UserId}", request.Id, request.UserId);
        return Result<GetJobTitleByIdQueryResponse>.Success(new GetJobTitleByIdQueryResponse(jobTitle), "Job title retrieved successfully");
    }
}