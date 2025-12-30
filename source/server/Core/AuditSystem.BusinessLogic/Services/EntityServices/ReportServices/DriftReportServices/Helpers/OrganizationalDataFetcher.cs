using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Models.Reports.DriftReport;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;

/// <summary>
/// Fetches organizational data in parallel()
/// </summary>
public sealed class OrganizationalDataFetcher(
    ICompanyService companyService,
    ILocationService locationService,
    IDepartmentService departmentService,
    ISubDepartmentService subDepartmentService,
    ILogger<OrganizationalDataFetcher> logger
) : IOrganizationalDataFetcher
{
    public async Task<OrganizationalData> FetchAsync(GenerateDriftReportRequest request)
    {
        logger.LogDebug("Fetching organizational data");

        var companyTask = companyService.GetCompanyByIdAsync(request.CompanyId);
        var locationTask = locationService.GetLocationByIdAsync(request.LocationId, request.UserId);
        var departmentTask = departmentService.GetDepartmentByIdAsync(request.DepartmentId, request.UserId);
        var subDepartmentTask = subDepartmentService.GetSubDepartmentByIdAsync(request.SubDepartmentId, request.UserId);

        await Task.WhenAll(companyTask, locationTask, departmentTask, subDepartmentTask);
        logger.LogDebug("Organizational data fetched successfully");
        return new OrganizationalData(
            await companyTask,
            await locationTask,
            await departmentTask,
            await subDepartmentTask
        );
    }
}
