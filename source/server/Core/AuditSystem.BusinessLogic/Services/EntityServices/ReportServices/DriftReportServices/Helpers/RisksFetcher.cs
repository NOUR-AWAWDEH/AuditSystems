using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Models.Pagination;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;

public class RisksFetcher(
    IRiskService riskService,
    ILogger<RisksFetcher> logger
) : IRisksFetcher
{
    public async Task<IReadOnlyList<RiskModel>> FetchAsync(GenerateDriftReportRequest request, IEnumerable<Guid> observationIds)
    {
        var allRisks = new List<RiskModel>();

        foreach (var observationId in observationIds)
        {
            var result = await riskService.SearchByUserIdAsync(
                request.UserId, "", observationId, new Page(1, 100));

            if (result?.Items != null)
            {
                 allRisks.AddRange(result.Items);
            }
        }

        if (allRisks.Count == 0)
        {
            logger.LogWarning("No risks found for provided observations");
        }
        else 
        {
             logger.LogDebug("Fetched {riskCount} risks across {observationCount} observations", allRisks.Count, observationIds.Count());
        }
        
        return allRisks;
    }
}