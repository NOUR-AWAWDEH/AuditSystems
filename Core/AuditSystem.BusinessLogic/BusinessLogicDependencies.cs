using AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;
using AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;
using AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;
using AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.TasksServices;
using AuditSystem.BusinessLogic.Services.EntityServices.UserServices;
using AuditSystem.BusinessLogic.Services.Transaction;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
using AuditSystem.Contract.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace AuditSystem.BusinessLogic;

public static class BusinessLogicDependencies
{
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services
            .AddMapper()
            .AddServices();

    private static IServiceCollection AddMapper(this IServiceCollection services) =>
        services
            .AddAutoMapper(typeof(BusinessLogicDependencies).Assembly);

    private static IServiceCollection AddServices(this IServiceCollection services) =>
    services
        .AddScoped<ITransaction, Transaction>()
        .AddScoped<IRiskService, RiskService>()
        // Audit Services
        .AddScoped<IAuditDomainService, AuditDomainService>()
        .AddScoped<IAuditEngagementService, AuditEngagementService>()
        .AddScoped<IAuditPlanSummaryService, AuditPlanSummaryService>()
        .AddScoped<IAuditUniverseObjectiveService, AuditUniverseObjectiveService>()
        .AddScoped<IAuditUniverseService, AuditUniverseService>()
        .AddScoped<IBusinessObjectiveService, BusinessObjectiveService>()
        .AddScoped<ISpecialProjectService, SpecialProjectService>()
        // Checklist Services
        .AddScoped<IChecklistService, ChecklistService>()
        .AddScoped<IChecklistManagementService, ChecklistManagementService>()
        .AddScoped<IRemarkService, RemarkService>()
        // Common Services
        .AddScoped<IRatingService, RatingService>()
        // Compliance Services
        .AddScoped<IComplianceAuditLinkService, ComplianceAuditLinkService>()
        .AddScoped<IComplianceChecklistService, ComplianceChecklistService>()
        // Jobs Services
        .AddScoped<IAuditJobService, AuditJobService>()
        .AddScoped<IJobPrioritizationService, JobPrioritizationService>()
        .AddScoped<IJobSchedulingService, JobSchedulingService>()
        // Organization Services
        .AddScoped<ICompanyService, CompanyService>()
        .AddScoped<IDepartmentService, DepartmentService>()
        .AddScoped<ILocationService, LocationService>()
        .AddScoped<ISubDepartmentService, SubDepartmentService>()
        .AddScoped<ISubLocationService, SubLocationService>()
        // Processes Services
        .AddScoped<IAuditProcessService, AuditProcessService>()
        .AddScoped<ISubProcessService, SubProcessService>()
        // Reports Services
        .AddScoped<IAuditExceptionRepotService, AuditExceptionRepotService>()
        .AddScoped<IAuditPlanSummaryReportService, AuditPlanSummaryReportService>()
        .AddScoped<IInternalAuditConsolidationReportService, InternalAuditConsolidationReportService>()
        .AddScoped<IJobTimeAllocationReportService, JobTimeAllocationReportService>()
        .AddScoped<IPlanningReportService, PlanningReportService>()
        .AddScoped<IReportingFollowUpService, ReportingFollowUpService>()
        // Risk Control Services
        .AddScoped<IRiskControlMatrixService, RiskControlMatrixService>()
        .AddScoped<IRiskControlService, RiskControlService>()
        .AddScoped<IRiskProgramService, RiskProgramService>()
        // Risks Services
        .AddScoped<IRiskAssessmentService, RiskAssessmentService>()
        .AddScoped<IRiskFactorService, RiskFactorService>()
        .AddScoped<ISpecificRiskFactorService, SpecificRiskFactorService>()
        // Skills Services
        .AddScoped<ISkillService, SkillService>()
        .AddScoped<ISkillSetService, SkillSetService>()
        // Supporting Docs Services
        .AddScoped<ISupportingDocService, SupportingDocService>()
        // Tasks Services
        .AddScoped<ITaskManagementService, TaskManagementService>()
        // Users Services
        .AddScoped<IUserManagementService, UserManagementService>();
}