using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Create;

internal sealed class CreateJobSchedulingCommandHandler(
    IJobSchedulingService jobSchedulingService,
    IMapper mapper,
    ILogger<CreateJobSchedulingCommandHandler> logger)
    : IRequestHandler<CreateJobSchedulingCommand, Result<CreateJobSchedulingCommandResponse>>
{
    public async Task<Result<CreateJobSchedulingCommandResponse>> Handle(CreateJobSchedulingCommand request, CancellationToken cancellationToken)
    {
        var jobSchedulingModel = mapper.Map<JobSchedulingModel>(request);
        var jobSchedulingId = await jobSchedulingService.CreateJobSchedulingAsync(jobSchedulingModel);
        logger.LogInformation("Job Scheduling with Auditable Unit {JobSchedulingAuditableUnit} has been created.", request.AuditableUnit);

        return Result<CreateJobSchedulingCommandResponse>.Created(new CreateJobSchedulingCommandResponse(jobSchedulingId));
    }
}
