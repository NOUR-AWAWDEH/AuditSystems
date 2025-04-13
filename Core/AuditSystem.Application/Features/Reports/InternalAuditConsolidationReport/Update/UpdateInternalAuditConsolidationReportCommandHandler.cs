using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Update;

internal sealed class UpdateInternalAuditConsolidationReportCommandHandler(
    IInternalAuditConsolidationReportService internalAuditConsolidationReportService,
    IMapper mapper,
    ILogger<UpdateInternalAuditConsolidationReportCommandHandler> logger)
    : IRequestHandler<UpdateInternalAuditConsolidationReportCommand, Result>
{
    public async Task<Result> Handle(UpdateInternalAuditConsolidationReportCommand request, CancellationToken cancellationToken)
    {
        var internalAuditConsolidationReportModel = mapper.Map<InternalAuditConsolidationReportModel>(request);
        await internalAuditConsolidationReportService.UpdateInternalAuditConsolidationReportAsync(internalAuditConsolidationReportModel);
        
        logger.LogInformation("Internal audit consolidation report updated successfully");
        return Result.Success();
    }
}
