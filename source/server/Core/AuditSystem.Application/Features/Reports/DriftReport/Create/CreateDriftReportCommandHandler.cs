using Ardalis.Result;
using AuditSystem.Application.Features.Reports.DriftReport.Create;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.DriftReport.Create;

internal sealed class CreateDriftReportCommandHandler(
    IDriftReportService driftReportService,
    IMapper mapper,
    ILogger<CreateDriftReportCommandHandler> logger
) : IRequestHandler<CreateDriftReportCommand, Result<CreateDriftReportCommandResponse>>
{
    public async Task<Result<CreateDriftReportCommandResponse>> Handle(
        CreateDriftReportCommand request,
        CancellationToken cancellationToken)
    {
        // Map command → service request
        var reportRequest = mapper.Map<GenerateDriftReportRequest>(request);

        // Call service to create and get full report with all objects
        var fullReport = await driftReportService.CreateDriftReportAsync(reportRequest);

        logger.LogInformation(
            "Drift report for CompanyId {CompanyId}, LocationId {LocationId} has been created by user {UserId}",
            request.CompanyId, request.LocationId, request.UserId);

        // Map model → command response
        var response = mapper.Map<CreateDriftReportCommandResponse>(fullReport);

        return Result<CreateDriftReportCommandResponse>.Created(response, "Drift report created.");
    }
}
