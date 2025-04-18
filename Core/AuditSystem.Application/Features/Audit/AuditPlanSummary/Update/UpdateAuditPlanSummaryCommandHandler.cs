using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Update;

internal sealed class UpdateAuditPlanSummaryCommandHandler(
    IAuditPlanSummaryService auditPlanSummaryService,
    IMapper mapper,
    ILogger<UpdateAuditPlanSummaryCommandHandler> logger)
    : IRequestHandler<UpdateAuditPlanSummaryCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditPlanSummaryCommand request, CancellationToken cancellationToken)
    {
        var auditPlanSummaryModel = mapper.Map<AuditPlanSummaryModel>(request);
        await auditPlanSummaryService.UpdateAuditPlanSummaryAsync(auditPlanSummaryModel);

        logger.LogInformation("Audit plan summary updated successfully");
        return Result.Success();
    }
}
