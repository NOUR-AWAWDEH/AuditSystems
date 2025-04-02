using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Create;

internal sealed class CreateJobPrioritizationCommandHandler(
    IJobPrioritizationService jobPrioritizationService,
    IMapper mapper,
    ILogger<CreateJobPrioritizationCommandHandler> logger)
    : IRequestHandler<CreateJobPrioritizationCommand, Result<CreateJobPrioritizationCommandResponse>>
{
    public async Task<Result<CreateJobPrioritizationCommandResponse>> Handle(CreateJobPrioritizationCommand request, CancellationToken cancellationToken)
    {
        var jobPrioritizationModel = mapper.Map<JobPrioritizationModel>(request);
        var jobPrioritizationId = await jobPrioritizationService.CreateJobPrioritizationAsync(jobPrioritizationModel);
        logger.LogInformation("Job Prioritization with Auditable Unit {AuditableUnit} has been created.", request.AuditableUnit);

        return Result<CreateJobPrioritizationCommandResponse>.Created(new CreateJobPrioritizationCommandResponse(jobPrioritizationId));
    }
}