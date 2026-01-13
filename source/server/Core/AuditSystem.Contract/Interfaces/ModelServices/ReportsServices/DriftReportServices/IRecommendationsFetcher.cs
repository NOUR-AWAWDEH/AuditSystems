using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.Reports.DriftReport;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IRecommendationsFetcher
{
    Task<IReadOnlyList<RecommendationModel>> FetchAsync(GenerateDriftReportRequest request, IReadOnlyList<ObservationModel> observations);
}