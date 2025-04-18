using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Update;

internal sealed class UpdateReportingFollowUpCommandHandler(
    IReportingFollowUpService reportingFollowUpService,
    IMapper mapper,
    ILogger<UpdateReportingFollowUpCommandHandler> logger)
    : IRequestHandler<UpdateReportingFollowUpCommand, Result>
{
    public async Task<Result> Handle(UpdateReportingFollowUpCommand request, CancellationToken cancellationToken)
    {
        var reportingFollowUpModel = mapper.Map<ReportingFollowUpModel>(request);
        await reportingFollowUpService.UpdateReportingFollowUpAsync(reportingFollowUpModel);

        logger.LogInformation("Reporting follow-up updated successfully");
        return Result.Success();
    }
}
