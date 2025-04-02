using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Create;

internal sealed class CreateAuditEngagementCommandHandler(
    IAuditEngagementService auditEngagementService,
    IMapper mapper,
    ILogger<CreateAuditEngagementCommandHandler> logger) :
    IRequestHandler<CreateAuditEngagementCommand, Result<CreateAuditEngagementCommandResponse>>
{
    public async Task<Result<CreateAuditEngagementCommandResponse>> Handle(CreateAuditEngagementCommand request, CancellationToken cancellationToken)
    {
        var auditEngagementModel = mapper.Map<AuditEngagementModel>(request);
        var auditEngagementId = await auditEngagementService.CreateAuditEngagementAsync(auditEngagementModel);
        logger.LogInformation("Audit engagement with name {AuditEngagementName} has been created.", request.JobName);
        
        return Result<CreateAuditEngagementCommandResponse>.Created(new CreateAuditEngagementCommandResponse(auditEngagementId));
    }
}
