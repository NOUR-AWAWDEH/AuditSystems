
using AuditSystem.Contract.Models.Reports.DriftReport;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;

public interface IOrganizationalDataFetcher
{
    Task<OrganizationalData> FetchAsync(GenerateDriftReportRequest request);
}
