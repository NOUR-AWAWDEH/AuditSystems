using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Update;

internal sealed class UpdateJobPrioritizationCommandHandler(
    IJobPrioritizationService jobPrioritizationService,
    IMapper mapper,
    ILogger<UpdateJobPrioritizationCommandHandler> logger)
    : IRequestHandler<UpdateJobPrioritizationCommand, Result>
{
    public async Task<Result> Handle(UpdateJobPrioritizationCommand request, CancellationToken cancellationToken)
    {
        var jobPrioritizationModel = mapper.Map<JobPrioritizationModel>(request);
        await jobPrioritizationService.UpdateJobPrioritizationAsync(jobPrioritizationModel);

        logger.LogInformation("Job prioritization updated successfully");
        return Result.Success();
    }
}
