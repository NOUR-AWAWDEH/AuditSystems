using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;

public interface IPlanningReportService
{
    public Task<Guid> CreatePlanningReportAsync(PlanningReportModel planningReportModel);
    public Task UpdatePlanningReportAsync(PlanningReportModel planningReportModel);
    public Task DeletePlanningReportAsync(Guid planningReportId);
    public Task<PlanningReportModel> GetPlanningReportByIdAsync(Guid id);
}
