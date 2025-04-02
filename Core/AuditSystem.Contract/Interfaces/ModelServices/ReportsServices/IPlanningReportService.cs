using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IPlanningReportService
{
    public Task<Guid> CreatePlanningReportAsync(PlanningReportModel planningReportModel);
}
