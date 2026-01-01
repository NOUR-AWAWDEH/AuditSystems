using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.Risks;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;

public class RisksFetcher(
    IRiskService riskService,
    ILogger<RisksFetcher> logger
) : IRisksFetcher
{
    public async Task<IReadOnlyList<RiskModel>> FetchAsync(GenerateDriftReportRequest request)
    {
        var result = await riskService.SearchByUserIdAsync(
                request.UserId, "", request.ObjectiveId, null);

            if (result?.Items == null)
            {
                logger.LogWarning("No risks found or null result for audit program {AuditProgramId}", 
                    request.AuditProgramId);
                return [];
            }

            logger.LogDebug("Fetched {riskCount} risks", result.Items.Count);
            return result.Items;
    }
}