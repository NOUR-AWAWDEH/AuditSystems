using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;

internal sealed class CreateAuditPlanSummaryCommandHandler(
    IAuditPlanSummaryService auditPlanSummaryService,
    IMapper mapper,
    ILogger<CreateAuditPlanSummaryCommandHandler> logger)
    : IRequestHandler<CreateAuditPlanSummaryCommand, Result<CreateAuditPlanSummaryCommandResponse>>
{
    public async Task<Result<CreateAuditPlanSummaryCommandResponse>> Handle(CreateAuditPlanSummaryCommand request, CancellationToken cancellationToken)
    {
        var auditPlanSummary = mapper.Map<AuditPlanSummaryModel>(request);
        var auditPlanSummaryId = await auditPlanSummaryService.CreateAuditPlanSummaryAsync(auditPlanSummary);
        logger.LogInformation($"Audit Plan Summary created with Component: {request.Component}");

        return Result<CreateAuditPlanSummaryCommandResponse>.Success(new CreateAuditPlanSummaryCommandResponse(auditPlanSummaryId));
    }
}
