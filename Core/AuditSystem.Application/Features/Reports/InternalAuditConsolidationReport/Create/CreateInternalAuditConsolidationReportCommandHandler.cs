using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Create;

internal sealed class CreateInternalAuditConsolidationReportCommandHandler(
    IInternalAuditConsolidationReportService internalAuditConsolidationReportService,
    IMapper mapper,
    ILogger<CreateInternalAuditConsolidationReportCommandHandler> logger)
    : IRequestHandler<CreateInternalAuditConsolidationReportCommand, Result<CreateInternalAuditConsolidationReportCommandResponse>>
{
    public async Task<Result<CreateInternalAuditConsolidationReportCommandResponse>> Handle(CreateInternalAuditConsolidationReportCommand request, CancellationToken cancellationToken)
    {
        var internalAuditConsolidationReportModel = mapper.Map<InternalAuditConsolidationReportModel>(request);
        var internalAuditConsolidationReportId = await internalAuditConsolidationReportService.CreateInternalAuditConsolidationReportAsync(internalAuditConsolidationReportModel);
        logger.LogInformation("Internal Audit Consolidation Report with Job Name {InternalAuditConsolidationReportJobName} has been created.", request.JobName);

        return Result<CreateInternalAuditConsolidationReportCommandResponse>.Created(new CreateInternalAuditConsolidationReportCommandResponse(internalAuditConsolidationReportId));
    }
}
