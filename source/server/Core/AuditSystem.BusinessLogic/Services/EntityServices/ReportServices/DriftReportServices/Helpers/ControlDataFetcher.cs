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
        
        var objectiveTask = objectiveService.GetObjectiveByIdAsync(request.ObjectiveId, request.UserId);
        var risks = risksFetcher.FetchAsync(request);
        var auditProgramTask = auditProgramService.GetAuditProgramByIdAsync(request.AuditProgramId, request.UserId);
        var observations = observationsFetcher.FetchAsync(request);

        await Task.WhenAll(objectiveTask, risks, auditProgramTask, observations);

        logger.LogDebug("Fetching control data succeeded");
        
        return new ControlData(
            objectiveTask.Result,
            risks.Result,
            auditProgramTask.Result,
            observations.Result
        );
    }
}