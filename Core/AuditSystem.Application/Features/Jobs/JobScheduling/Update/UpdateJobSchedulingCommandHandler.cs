using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Update;

internal sealed class UpdateJobSchedulingCommandHandler(
    IJobSchedulingService jobSchedulingService,
    IMapper mapper,
    ILogger<UpdateJobSchedulingCommandHandler> logger)
    : IRequestHandler<UpdateJobSchedulingCommand, Result>
{
    public async Task<Result> Handle(UpdateJobSchedulingCommand request, CancellationToken cancellationToken)
    {
        var jobSchedulingModel = mapper.Map<JobSchedulingModel>(request);
        await jobSchedulingService.UpdateJobSchedulingAsync(jobSchedulingModel);

        logger.LogInformation("Job scheduling updated successfully");
        return Result.Success();
    }
}
