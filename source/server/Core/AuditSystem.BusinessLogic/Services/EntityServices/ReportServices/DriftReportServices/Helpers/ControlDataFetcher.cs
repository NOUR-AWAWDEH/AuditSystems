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
    IRiskService riskService,
    IAuditProgramService auditProgramService,
    IObservationService observationService,
    ILogger<ControlDataFetcher> logger
) : IControlDataFetcher
{
    public async Task<ControlData> FetchAsync(GenerateDriftReportRequest request)
    {
        logger.LogDebug("Fetching control data");
        
        var objectiveTask = objectiveService.GetObjectiveByIdAsync(request.ObjectiveId, request.UserId);
        var risksTask = riskService.SearchByUserIdAsync(request.UserId, "", request.ObjectiveId, null);
        var auditProgramTask = auditProgramService.GetAuditProgramByIdAsync(request.AuditProgramId, request.UserId);
        var observationsTask = observationService.SearchByUserIdAsync(request.UserId, "", request.AuditProgramId, null);

        await Task.WhenAll(objectiveTask, risksTask, auditProgramTask, observationsTask);
        
        logger.LogDebug("Control data fetched successfully");
        
        return new ControlData(
            await objectiveTask,
            await risksTask,
            await auditProgramTask,
            await observationsTask
        );
    }
}