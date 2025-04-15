using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Update;

internal sealed class UpdateAuditExceptionReportCommandHandler(
    IAuditExceptionReportService auditExceptionRepotService,
    IMapper mapper,
    ILogger<UpdateAuditExceptionReportCommandHandler> logger)
    : IRequestHandler<UpdateAuditExceptionReportCommand, Result>
{
    public async Task<Result> Handle(UpdateAuditExceptionReportCommand request, CancellationToken cancellationToken)
    {
        var auditExceptionReportModel = mapper.Map<AuditExceptionReportModel>(request);
        await auditExceptionRepotService.UpdateAuditExceptionReportAsync(auditExceptionReportModel);

        logger.LogInformation("Audit exception report updated successfully");
        return Result.Success();
    }
}