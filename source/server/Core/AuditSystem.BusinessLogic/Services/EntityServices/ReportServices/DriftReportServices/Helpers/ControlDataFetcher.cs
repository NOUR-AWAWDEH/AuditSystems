using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Reports.DriftReport;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;

/// <summary>
/// Fetches control-related data (Objective, Risks, AuditProgram, Observations)
/// </summary>
public sealed class ControlDataFetcher(
    IObjectiveService objectiveService,
    IRisksFetcher risksFetcher,
    IAuditProgramService auditProgramService,
    IObservationsFetcher observationsFetcher,
    ILogger<ControlDataFetcher> logger
) : IControlDataFetcher
{
    public async Task<ControlData> FetchAsync(GenerateDriftReportRequest request)
    {
        logger.LogDebug("Fetching control data");
        
        var objective = await objectiveService.GetObjectiveByIdAsync(request.ObjectiveId, request.UserId);
        var auditProgram = await auditProgramService.GetAuditProgramByIdAsync(request.AuditProgramId, request.UserId);
        var observations = await observationsFetcher.FetchAsync(request);
        var risks = await risksFetcher.FetchAsync(request, observations.Select(o => o.Id));

        logger.LogDebug("Fetching control data succeeded");
        
        return new ControlData(
            objective,
            risks,
            auditProgram,
            observations
        );
    }
}