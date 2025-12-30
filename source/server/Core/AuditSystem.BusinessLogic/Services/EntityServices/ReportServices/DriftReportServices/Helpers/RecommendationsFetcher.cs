using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;

/// <summary>
/// Fetches recommendations for given observations
/// </summary>
public sealed class RecommendationsFetcher(
    IRecommendationService recommendationService,
    ILogger<RecommendationsFetcher> logger
) : IRecommendationsFetcher
{
    public async Task<List<RecommendationModel>> FetchAsync(GenerateDriftReportRequest request, IReadOnlyList<ObservationModel> observations)
    {
        if (observations.Count == 0)
        {
            logger.LogDebug("No observations found, skipping recommendations fetch");
            return [];
        }

        logger.LogDebug("Fetching recommendations for {ObservationCount} observations", observations.Count);

        var recommendations = new List<RecommendationModel>();

        foreach (var observation in observations)
        {
            var result = await recommendationService.SearchByUserIdAsync(
                request.UserId, "", observation.Id, null);
            recommendations.AddRange(result.Items);
        }

        logger.LogDebug("Fetched {RecommendationCount} recommendations total", recommendations.Count);
        return recommendations;
    }
}
