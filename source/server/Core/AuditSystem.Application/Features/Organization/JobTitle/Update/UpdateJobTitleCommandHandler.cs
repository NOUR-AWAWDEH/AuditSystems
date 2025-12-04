using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.JobTitle.Update;

internal sealed class UpdateJobTitleCommandHandler(
    IJobTitleService jobTitleService,
    IMapper mapper,
    ILogger<UpdateJobTitleCommandHandler> logger)
    : IRequestHandler<UpdateJobTitleCommand, Result>
{
    public async Task<Result> Handle(UpdateJobTitleCommand request, CancellationToken cancellationToken)
    {
        var jobTitleModel = mapper.Map<JobTitleModel>(request);
        await jobTitleService.UpdateJobTitleAsync(jobTitleModel, request.UserId);

        logger.LogInformation("Job Title with ID: {JobTitleId} updated by User Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Job Title updated successfully");
    }
}
