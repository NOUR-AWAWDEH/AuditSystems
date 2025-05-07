using AuditSystem.BusinessLogic.Mappings;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ChecklistServices;
using AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;
using AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;
using AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.TasksServices;
using AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserDesignationServices;
using AuditSystem.BusinessLogic.Services.Transaction;
using AuditSystem.Contract.Interfaces.Cache;
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
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Contract.Models.Tasks;
using AuditSystem.Contract.Models.UserDesignation;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Checklists;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Compliance;
using AuditSystem.Domain.Entities.Jobs;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.Reports;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.Skills;
using AuditSystem.Domain.Entities.SupportingDocs;
using AuditSystem.Domain.Entities.Tasks;
using AuditSystem.Domain.Entities.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AuditSystem.BusinessLogic;

public static class BusinessLogicDependencies
{
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services
            .AddMappers()
            .AddServices();

    private static IServiceCollection AddMappers(this IServiceCollection services) =>
    services.AddAutoMapper(
        typeof(BusinessLogicDependencies).Assembly,
        typeof(AuditMappingProfile).Assembly,
        typeof(ChecklistMappingProfile).Assembly,
        typeof(CommonMappingProfile).Assembly,
        typeof(ComplianceMappingProfile).Assembly,
        typeof(JobsMappingProfile).Assembly,
        typeof(OrganizationMappingProfile).Assembly,
        typeof(ProcessMappingProfile).Assembly,
        typeof(ReportsMappingProfile).Assembly,
        typeof(RiskControlsMappingProfile).Assembly,
        typeof(RisksMappingProfile).Assembly,
        typeof(SkillsMappingProfile).Assembly,
        typeof(SupportingDocsMappingProfile).Assembly,
        typeof(TaskManagementMappingProfile).Assembly,
        typeof(UserDesignationMappingProfile).Assembly
    );

    private static IServiceCollection AddServices(this IServiceCollection services) =>
    services
        .AddScoped<ITransaction, Transaction>()
        .AddScoped<IRiskService, RiskService>()
        // Add MemoryCache here
        .AddMemoryCache()

        // Audit Services
        .AddScoped<IAuditDomainService, AuditDomainService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditDomain, AuditDomainModel>, AuditDomainService>()

        .AddScoped<IAuditEngagementService, AuditEngagementService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditEngagement, AuditEngagementModel>, AuditEngagementService>()
        
        .AddScoped<IAuditPlanSummaryService, AuditPlanSummaryService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditPlanSummary, AuditPlanSummaryModel>, AuditPlanSummaryService>()
        
        .AddScoped<IAuditUniverseObjectiveService, AuditUniverseObjectiveService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditUniverseObjective, AuditUniverseObjectiveModel>, AuditUniverseObjectiveService>()
        
        .AddScoped<IAuditUniverseService, AuditUniverseService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditUniverse, AuditUniverseModel>, AuditUniverseService>()
        
        .AddScoped<IBusinessObjectiveService, BusinessObjectiveService>()
        .AddScoped<IUnifiedCacheService<Guid, BusinessObjective, BusinessObjectiveModel>, BusinessObjectiveService>()
        
        .AddScoped<ISpecialProjectService, SpecialProjectService>()
        .AddScoped<IUnifiedCacheService<Guid, SpecialProject, SpecialProjectModel>, SpecialProjectService>()
        
        .AddScoped<IObjectiveService, ObjectiveService>()
        .AddScoped<IUnifiedCacheService<Guid, Objective, ObjectiveModel>, ObjectiveService>()

        // Checklist Services
        .AddScoped<IChecklistService, ChecklistService>()
        .AddScoped<IUnifiedCacheService<Guid, Checklist, ChecklistModel>, ChecklistService>()
        
        .AddScoped<IRemarkService, RemarkService>()
        .AddScoped<IUnifiedCacheService<Guid, Remark, RemarkModel>, RemarkService>()

        // Common Services
        .AddScoped<IRatingService, RatingService>()
        .AddScoped<IUnifiedCacheService<Guid, Rating, RatingModel>, RatingService>()

        // Compliance Services
        .AddScoped<IComplianceChecklistService, ComplianceChecklistService>()
        .AddScoped<IUnifiedCacheService<Guid, ComplianceChecklist, ComplianceChecklistModel>, ComplianceChecklistService>()

        // Jobs Services
        .AddScoped<IAuditJobService, AuditJobService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditJob, AuditJobModel>, AuditJobService>()
        
        .AddScoped<IJobPrioritizationService, JobPrioritizationService>()
        .AddScoped<IUnifiedCacheService<Guid, JobPrioritization, JobPrioritizationModel>, JobPrioritizationService>()
        
        .AddScoped<IJobSchedulingService, JobSchedulingService>()
        .AddScoped<IUnifiedCacheService<Guid, JobScheduling, JobSchedulingModel>, JobSchedulingService>()

        // Organization Services
        .AddScoped<ICompanyService, CompanyService>()
        .AddScoped<IUnifiedCacheService<Guid, Company, CompanyModel>, CompanyService>()
        
        .AddScoped<IDepartmentService, DepartmentService>()
        .AddScoped<IUnifiedCacheService<Guid, Department, DepartmentModel>, DepartmentService>()
        
        .AddScoped<ILocationService, LocationService>()
        .AddScoped<IUnifiedCacheService<Guid, Location, LocationModel>, LocationService>()
        
        .AddScoped<ISubDepartmentService, SubDepartmentService>()
        .AddScoped<IUnifiedCacheService<Guid, SubDepartment, SubDepartmentModel>, SubDepartmentService>()
        
        .AddScoped<ISubLocationService, SubLocationService>()
        .AddScoped<IUnifiedCacheService<Guid, SubLocation, SubLocationModel>, SubLocationService>()

        // Processes Services
        .AddScoped<IAuditProcessService, AuditProcessService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditProcess, AuditProcessModel>, AuditProcessService>()
        
        .AddScoped<ISubProcessService, SubProcessService>()
        .AddScoped<IUnifiedCacheService<Guid, SubProcess, SubProcessModel>, SubProcessService>()

        // Reports Services
        .AddScoped<IAuditExceptionReportService, AuditExceptionRepotService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditExceptionReport, AuditExceptionReportModel>, AuditExceptionRepotService>()
        
        .AddScoped<IAuditPlanSummaryReportService, AuditPlanSummaryReportService>()
        .AddScoped<IUnifiedCacheService<Guid, AuditPlanSummaryReport, AuditPlanSummaryReportModel>, AuditPlanSummaryReportService>()
        
        .AddScoped<IInternalAuditConsolidationReportService, InternalAuditConsolidationReportService>()
        .AddScoped<IUnifiedCacheService<Guid, InternalAuditConsolidationReport, InternalAuditConsolidationReportModel>, InternalAuditConsolidationReportService>()
        
        .AddScoped<IJobTimeAllocationReportService, JobTimeAllocationReportService>()
        .AddScoped<IUnifiedCacheService<Guid, JobTimeAllocationReport, JobTimeAllocationReportModel>, JobTimeAllocationReportService>()
        
        .AddScoped<IPlanningReportService, PlanningReportService>()
        .AddScoped<IUnifiedCacheService<Guid, PlanningReport, PlanningReportModel>, PlanningReportService>()
        
        .AddScoped<IReportingFollowUpService, ReportingFollowUpService>()
        .AddScoped<IUnifiedCacheService<Guid, ReportingFollowUp, ReportingFollowUpModel>, ReportingFollowUpService>()

        // Risk Control Services
        .AddScoped<IRiskControlMatrixService, RiskControlMatrixService>()
        .AddScoped<IUnifiedCacheService<Guid, RiskControlMatrix, RiskControlMatrixModel>, RiskControlMatrixService>()
        
        .AddScoped<IRiskControlService, RiskControlService>()
        .AddScoped<IUnifiedCacheService<Guid, RiskControls, RiskControlsModel>, RiskControlService>()
        
        .AddScoped<IRiskProgramService, RiskProgramService>()
        .AddScoped<IUnifiedCacheService<Guid, RiskProgram, RiskProgramModel>, RiskProgramService>()

        // Risks Services
        .AddScoped<IRiskAssessmentService, RiskAssessmentService>()
        .AddScoped<IUnifiedCacheService<Guid, RiskAssessment, RiskAssessmentModel>, RiskAssessmentService>()
        
        .AddScoped<IRiskFactorService, RiskFactorService>()
        .AddScoped<IUnifiedCacheService<Guid, RiskFactor, RiskFactorModel>, RiskFactorService>()

        // Skills Services
        .AddScoped<ISkillService, SkillService>()
        .AddScoped<IUnifiedCacheService<Guid, Skill, SkillModel>, SkillService>()
        
        .AddScoped<ISkillSetService, SkillSetService>()
        .AddScoped<IUnifiedCacheService<Guid, SkillSet, SkillSetModel>, SkillSetService>()
        
        .AddScoped<ISkillCategoryService, SkillCategoryService>()
        .AddScoped<IUnifiedCacheService<Guid, SkillCategory, SkillCategoryModel>, SkillCategoryService>()

        // Supporting Docs Services
        .AddScoped<ISupportingDocService, SupportingDocService>()
        .AddScoped<IUnifiedCacheService<Guid, SupportingDoc, SupportingDocModel>, SupportingDocService>()

        // Tasks Services
        .AddScoped<ITaskManagementService, TaskManagementService>()
        .AddScoped<IUnifiedCacheService<Guid, TaskManagement, TaskManagementModel>, TaskManagementService>()

        // User Designation Services
        .AddScoped<IUserDesignationService, UserDesignationService>()
        .AddScoped<IUnifiedCacheService<Guid, UserDesignation, UserDesignationModel>, UserDesignationService>();
    
}
