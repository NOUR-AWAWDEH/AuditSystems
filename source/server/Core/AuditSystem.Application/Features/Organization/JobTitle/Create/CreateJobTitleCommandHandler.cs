using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.JobTitle.Create;

public sealed class CreateJobTitleCommandHandler(
    IJobTitleService jobTitleService,
    IMapper mapper,
    ILogger<CreateJobTitleCommandHandler> logger)
    : IRequestHandler<CreateJobTitleCommand, Result<CreateJobTitleCommandResponse>>
{
    public async Task<Result<CreateJobTitleCommandResponse>> Handle(CreateJobTitleCommand request, CancellationToken cancellationToken)
    {
        var jobTitleModel = mapper.Map<JobTitleModel>(request);
        var jobTitleId = await jobTitleService.CreateJobTitleAsync(jobTitleModel, request.UserId);
        
        logger.LogInformation("JobTitle with Name {JobTitleName} has been created by user with id: {UserId}", request.Name, request.UserId);
        return Result<CreateJobTitleCommandResponse>.Created(new(jobTitleId), "JobTitle created.");
    }
}