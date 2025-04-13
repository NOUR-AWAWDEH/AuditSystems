using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Update;

internal sealed class UpdateAuditJobCommandHandler(
    IAuditJobService auditJobService,
    IMapper mapper,
    ILogger<UpdateAuditJobCommandHandler> logger)
    : IRequestHandler<UpdateAuditJobCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditJobCommand request, CancellationToken cancellationToken)
    {
        var auditJobModel = mapper.Map<AuditJobModel>(request);
        await auditJobService.UpdateAuditJobAsync(auditJobModel);

        logger.LogInformation("Audit job updated successfully");
        return Result.Success();
    }
}