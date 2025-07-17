using AuditSystem.BusinessLogic.Mappings;
using AuditSystem.BusinessLogic.Services.DownloadFile;
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
using AuditSystem.BusinessLogic.Services.FileStorageServices;
using AuditSystem.BusinessLogic.Services.Transaction;
using AuditSystem.BusinessLogic.Services.UrlServices;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubLocationHelpers;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Download;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
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
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AuditSystem.BusinessLogic.Helpers.SupportingDocsHelpers;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;

namespace AuditSystem.BusinessLogic;

public static class BusinessLogicDependencies
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        var fileStorageSettings = new FileStorageSettings
        {
            UploadPath = Environment.GetEnvironmentVariable("FILESTORAGE_UPLOADPATH")
                ?? throw new InvalidOperationException("Missing FILESTORAGE_UPLOADPATH"),

            PublicBaseUrl = Environment.GetEnvironmentVariable("FILE_STORAGE_BASE_URL")
                ?? throw new InvalidOperationException("Missing FILE_STORAGE_BASE_URL"),

            SignedUrlSecret = Environment.GetEnvironmentVariable("SIGNED_URL_SECRET_KEY")
                ?? throw new InvalidOperationException("Missing SIGNED_URL_SECRET_KEY in environment variables or .env file.")
        };
        
        // Register all other services
        services
            .AddMappers()
            .AddSingleton(fileStorageSettings)
            .AddScoped<IFileStorage, LocalFileStorage>()
            .AddScoped<ITransaction, Transaction>()
            .AddScoped<IRiskService, RiskService>()
            .AddMemoryCache()

            // Audit
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

            // Checklist
            .AddScoped<IChecklistService, ChecklistService>()
            .AddScoped<IUnifiedCacheService<Guid, Checklist, ChecklistModel>, ChecklistService>()
            .AddScoped<IRemarkService, RemarkService>()
            .AddScoped<IUnifiedCacheService<Guid, Remark, RemarkModel>, RemarkService>()

            // Common
            .AddScoped<IRatingService, RatingService>()
            .AddScoped<IUnifiedCacheService<Guid, Rating, RatingModel>, RatingService>()

            // Compliance
            .AddScoped<IComplianceChecklistService, ComplianceChecklistService>()
            .AddScoped<IUnifiedCacheService<Guid, ComplianceChecklist, ComplianceChecklistModel>, ComplianceChecklistService>()

            // Jobs
            .AddScoped<IAuditJobService, AuditJobService>()
            .AddScoped<IUnifiedCacheService<Guid, AuditJob, AuditJobModel>, AuditJobService>()
            .AddScoped<IJobPrioritizationService, JobPrioritizationService>()
            .AddScoped<IUnifiedCacheService<Guid, JobPrioritization, JobPrioritizationModel>, JobPrioritizationService>()
            .AddScoped<IJobSchedulingService, JobSchedulingService>()
            .AddScoped<IUnifiedCacheService<Guid, JobScheduling, JobSchedulingModel>, JobSchedulingService>()

            // Organization
            //Company
            .AddScoped<ICompanyService, CompanyService>()
            .AddScoped<IUnifiedCacheService<Guid, Company, CompanyModel>, CompanyService>()
            
            // Department
            .AddScoped<IDepartmentService, DepartmentService>()
            .AddScoped<IUnifiedCacheService<Guid, Department, DepartmentModel>, DepartmentService>()
            .AddScoped<IDepartmentValidation, DepartmentValidationHelper>()
            .AddScoped<IDepartmentSearch, DepartmentSearchHelper>()

            //Location
            .AddScoped<ILocationService, LocationService>()
            .AddScoped<IUnifiedCacheService<Guid, Location, LocationModel>, LocationService>()
            .AddScoped<ILocationValidation, LocationValidationHelper>()
            .AddScoped<ILocationSearch, LocationSearchHelper>()

            //SubDepartment
            .AddScoped<ISubDepartmentService, SubDepartmentService>()
            .AddScoped<IUnifiedCacheService<Guid, SubDepartment, SubDepartmentModel>, SubDepartmentService>()
            .AddScoped<ISubDepartmentValidation, SubDepartmentValidationHelper>()
            .AddScoped<ISubDepartmentSearch, SubDepartmentSearchHelper>()
            
            // SubLocation
            .AddScoped<ISubLocationService, SubLocationService>()
            .AddScoped<IUnifiedCacheService<Guid, SubLocation, SubLocationModel>, SubLocationService>()
            .AddScoped<ISubLocationValidation, SubLocationValidationHelper>()
            .AddScoped<ISubLocationSearch, SubLocationSearchHelper>()

            // Processes
            .AddScoped<IAuditProcessService, AuditProcessService>()
            .AddScoped<IUnifiedCacheService<Guid, AuditProcess, AuditProcessModel>, AuditProcessService>()
            .AddScoped<ISubProcessService, SubProcessService>()
            .AddScoped<IUnifiedCacheService<Guid, SubProcess, SubProcessModel>, SubProcessService>()

            // Reports
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

            // Risk Controls
            .AddScoped<IRiskControlMatrixService, RiskControlMatrixService>()
            .AddScoped<IUnifiedCacheService<Guid, RiskControlMatrix, RiskControlMatrixModel>, RiskControlMatrixService>()
            .AddScoped<IRiskControlService, RiskControlService>()
            .AddScoped<IUnifiedCacheService<Guid, RiskControls, RiskControlsModel>, RiskControlService>()
            .AddScoped<IRiskProgramService, RiskProgramService>()
            .AddScoped<IUnifiedCacheService<Guid, RiskProgram, RiskProgramModel>, RiskProgramService>()

            // Risks
            .AddScoped<IRiskAssessmentService, RiskAssessmentService>()
            .AddScoped<IUnifiedCacheService<Guid, RiskAssessment, RiskAssessmentModel>, RiskAssessmentService>()
            .AddScoped<IRiskFactorService, RiskFactorService>()
            .AddScoped<IUnifiedCacheService<Guid, RiskFactor, RiskFactorModel>, RiskFactorService>()

            // Skills
            .AddScoped<ISkillService, SkillService>()
            .AddScoped<IUnifiedCacheService<Guid, Skill, SkillModel>, SkillService>()
            .AddScoped<ISkillSetService, SkillSetService>()
            .AddScoped<IUnifiedCacheService<Guid, SkillSet, SkillSetModel>, SkillSetService>()
            .AddScoped<ISkillCategoryService, SkillCategoryService>()
            .AddScoped<IUnifiedCacheService<Guid, SkillCategory, SkillCategoryModel>, SkillCategoryService>()

            // Supporting Docs
            .AddScoped<ISupportingDocService, SupportingDocService>()
            .AddScoped<IUnifiedCacheService<Guid, SupportingDoc, SupportingDocModel>, SupportingDocService>()
            .AddScoped<ISupportingDocValidation, SupportingDocValidationHelper>()
            .AddScoped<ISupportingDocSearch, SupportingDocSearchHelper>()
            
            // File Path Builder
            .AddSingleton<IFilePathBuilder, FilePathBuilder>()

            // Url
            .AddSingleton<ISignedUrlService, SignedUrlService>()
            .AddScoped<IURLService,URLService>()

            // Download Service
            .AddScoped<IDecodeDownloadService, DecodeDownloadService>()
            
            // Tasks
            .AddScoped<ITaskManagementService, TaskManagementService>()
            .AddScoped<IUnifiedCacheService<Guid, TaskManagement, TaskManagementModel>, TaskManagementService>()

            // User Designation
            .AddScoped<IUserDesignationService, UserDesignationService>()
            .AddScoped<IUnifiedCacheService<Guid, UserDesignation, UserDesignationModel>, UserDesignationService>();

        return services;
    }

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
}
