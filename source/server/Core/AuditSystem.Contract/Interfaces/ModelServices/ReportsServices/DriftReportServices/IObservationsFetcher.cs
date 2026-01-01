using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Reports.DriftReport;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IObservationsFetcher
{
    Task<IReadOnlyList<ObservationModel>> FetchAsync(GenerateDriftReportRequest request);
}