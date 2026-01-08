using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IRisksFetcher
{
    Task<IReadOnlyList<RiskModel>> FetchAsync(GenerateDriftReportRequest request, IEnumerable<Guid> observationIds);
}
