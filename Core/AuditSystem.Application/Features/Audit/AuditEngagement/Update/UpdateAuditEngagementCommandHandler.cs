using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Update;

internal sealed class UpdateAuditEngagementCommandHandler(
    IAuditEngagementService auditEngagementService,
    IMapper mapper,
    ILogger<UpdateAuditEngagementCommandHandler> logger)
    : IRequestHandler<UpdateAuditEngagementCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditEngagementCommand request, CancellationToken cancellationToken)
    {
        var auditEngagementModel = mapper.Map<AuditEngagementModel>(request);
        await auditEngagementService.UpdateAuditEngagementAsync(auditEngagementModel);

        logger.LogInformation("Audit engagement updated successfully");
        return Result.Success();
    }
}
