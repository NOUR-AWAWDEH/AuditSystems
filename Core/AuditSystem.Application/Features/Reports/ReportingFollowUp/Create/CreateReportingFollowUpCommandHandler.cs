using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Create;

internal sealed class CreateReportingFollowUpCommandHandler(
    IReportingFollowUpService reportingFollowUpService,
    IMapper mapper,
    ILogger<CreateReportingFollowUpCommandHandler> logger)
    : IRequestHandler<CreateReportingFollowUpCommand, Result<CreateReportingFollowUpCommandResponse>>
{
    public async Task<Result<CreateReportingFollowUpCommandResponse>> Handle(CreateReportingFollowUpCommand request, CancellationToken cancellationToken)
    {
        var reportingFollowUpModel = mapper.Map<ReportingFollowUpModel>(request);
        var reportingFollowUpId = await reportingFollowUpService.CreateReportingFollowUpAsync(reportingFollowUpModel);
        logger.LogInformation("Reporting Follow Up with Reporting {ReportingFollowUpReporting} has been created.", request.Reporting);

        return Result<CreateReportingFollowUpCommandResponse>.Created(new CreateReportingFollowUpCommandResponse(reportingFollowUpId));
    }
}
