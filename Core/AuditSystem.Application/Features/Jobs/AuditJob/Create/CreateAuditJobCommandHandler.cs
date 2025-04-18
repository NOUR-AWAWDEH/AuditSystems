using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Create;

internal sealed class CreateAuditJobCommandHandler(
    IAuditJobService auditJobService,
    IMapper mapper,
    ILogger<CreateAuditJobCommandHandler> logger)
    : IRequestHandler<CreateAuditJobCommand, Result<CreateAuditJobCommandResponse>>
{
    public async Task<Result<CreateAuditJobCommandResponse>> Handle(CreateAuditJobCommand request, CancellationToken cancellationToken)
    {
        var auditJobModel = mapper.Map<AuditJobModel>(request);
        var auditJobId = await auditJobService.CreateAuditJobAsync(auditJobModel);
        logger.LogInformation("Audit Job with Job Name {AuditJobName} has been created.", request.JobName);

        return Result<CreateAuditJobCommandResponse>.Created(new(auditJobId));
    }
}
