using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;
public sealed class ObservationsFetcher(
    IObservationService observationService,
    ILogger<ObservationsFetcher> logger
) : IObservationsFetcher
{
    public async Task<IReadOnlyList<ObservationModel>> FetchAsync(GenerateDriftReportRequest request)
    {
            var result = await observationService.SearchByUserIdAsync(
                request.UserId, "", request.AuditProgramId, null);

            if (result?.Items == null)
            {
                logger.LogWarning("No observations found or null result for audit program {AuditProgramId}", 
                    request.AuditProgramId);
                return [];
            }

            logger.LogDebug("Fetched {ObservationCount} observations", result.Items.Count);
            return result.Items;
    }
}