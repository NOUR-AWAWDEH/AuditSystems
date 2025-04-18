using AuditSystem.BusinessLogic.Mappings;
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
using AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserDesignationServies;
using AuditSystem.BusinessLogic.Services.Transaction;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
using AuditSystem.Contract.Interfaces.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace AuditSystem.BusinessLogic;

public static class BusinessLogicDependencies
{
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services
            .AddMappers()
            .AddServices();

    private static IServiceCollection AddMappers(this IServiceCollection services) =>
    services
        .AddAutoMapper(typeof(BusinessLogicDependencies).Assembly)
        .AddAutoMapper(typeof(AuditMappingProfile).Assembly)
        .AddAutoMapper(typeof(ChecklistMappingProfile).Assembly)
        .AddAutoMapper(typeof(CommonMappingProfile).Assembly)
        .AddAutoMapper(typeof(ComplianceMappingProfile).Assembly)
        .AddAutoMapper(typeof(JobsMappingProfile).Assembly)
        .AddAutoMapper(typeof(OrganizationMappingProfile).Assembly)
        .AddAutoMapper(typeof(ProcessMappingProfile).Assembly)
        .AddAutoMapper(typeof(ReportsMappingProfile).Assembly)
        .AddAutoMapper(typeof(RiskControlsMappingProfile).Assembly)
        .AddAutoMapper(typeof(RisksMappingProfile).Assembly)
        .AddAutoMapper(typeof(SkillsMappingProfile).Assembly)
        .AddAutoMapper(typeof(SupportingDocsMappingProfile).Assembly)
        .AddAutoMapper(typeof(TaskManagementMappingProfile).Assembly)
        .AddAutoMapper(typeof(UserDesignationMappingProfile).Assembly);


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
        .AddScoped<IObjectiveService, ObjectiveService>()

        // Checklist Services
        .AddScoped<IChecklistService, ChecklistService>()
        .AddScoped<IRemarkService, RemarkService>()
        .AddAutoMapper(typeof(ChecklistMappingProfile).Assembly)

        // Common Services
        .AddScoped<IRatingService, RatingService>()

        // Compliance Services
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
        .AddScoped<IAuditExceptionReportService, AuditExceptionRepotService>()
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

        // Skills Services
        .AddScoped<ISkillService, SkillService>()
        .AddScoped<ISkillSetService, SkillSetService>()
        .AddScoped<ISkillCategoryService, SkillCategoryService>()

        // Supporting Docs Services
        .AddScoped<ISupportingDocService, SupportingDocService>()

        // Tasks Services
        .AddScoped<ITaskManagementService, TaskManagementService>()

        // User Desgination Services
        .AddScoped<IUserDesignationService, UserDesignationService>();
}