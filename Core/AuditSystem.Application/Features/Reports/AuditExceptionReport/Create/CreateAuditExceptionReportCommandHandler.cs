using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Create;

internal sealed class CreateAuditExceptionReportCommandHandler(
    IAuditExceptionReportService auditExceptionRepotService,
    IMapper mapper,
    ILogger<CreateAuditExceptionReportCommandHandler> logger)
    : IRequestHandler<CreateAuditExceptionReportCommand, Result<CreateAuditExceptionReportCommandResponse>>
{
    public async Task<Result<CreateAuditExceptionReportCommandResponse>> Handle(CreateAuditExceptionReportCommand request, CancellationToken cancellationToken)
    {
        var auditExceptionReportModel = mapper.Map<AuditExceptionReportModel>(request);
        var auditExceptionReportId = await auditExceptionRepotService.CreateAuditExceptionReportAsync(auditExceptionReportModel);
        logger.LogInformation("Audit Exception Report with Report Name {AuditExceptionReportName} has been created.", request.ReportName);

        return Result<CreateAuditExceptionReportCommandResponse>.Created(new CreateAuditExceptionReportCommandResponse(auditExceptionReportId));
    }
}
