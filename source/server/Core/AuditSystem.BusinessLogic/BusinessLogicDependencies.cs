using AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;
using AuditSystem.BusinessLogic.Helpers.AuditProgramHelpers;
using AuditSystem.BusinessLogic.Helpers.ControlDesignHelpers;
using AuditSystem.BusinessLogic.Helpers.ExitControlHelpers;
using AuditSystem.BusinessLogic.Helpers.ObservationHelpers;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.JobTitleHelpers;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;
using AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;
using AuditSystem.BusinessLogic.Helpers.RecommendationHelpers;
using AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.ObjectiveHelpers;
using AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.RiskControlMatrixHelpers;
using AuditSystem.BusinessLogic.Helpers.RiskHelpers;
using AuditSystem.BusinessLogic.Helpers.SubProcessHelpers;
using AuditSystem.BusinessLogic.Helpers.SupportingDocsHelpers;
using AuditSystem.BusinessLogic.Mappings;
using AuditSystem.BusinessLogic.Services.DownloadFile;
using AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;
using AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ObjectiveServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ObservationServices;
using AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;
using AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RecommendationServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;
using AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;
using AuditSystem.BusinessLogic.Services.EntityServices.UserServices;
using AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserSessionManager;
using AuditSystem.BusinessLogic.Services.FileStorageServices;
using AuditSystem.BusinessLogic.Services.Transaction;
using AuditSystem.BusinessLogic.Services.UrlServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.Download;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
using AuditSystem.Contract.Interfaces.Transaction;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.Recommendations;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.SupportingDocs;
using Microsoft.Extensions.DependencyInjection;

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


            // Common
            .AddScoped<IRateService, RateService>()
            .AddScoped<IUnifiedCacheService<Guid, Rate, RateModel>, RateService>()


            //Audit 
            .AddScoped<IAuditProgramService, AuditProgramService>()
            .AddScoped<IUnifiedCacheService<Guid, AuditProgram, AuditProgramModel>, AuditProgramService>()
            .AddScoped<IAuditProgramValidation, AuditProgramValidationHelper>()
            .AddScoped<IAuditProgramSearch, AuditProgramSearchHelper>()


            // Organization
            //Company
            .AddScoped<ICompanyService, CompanyService>()
            .AddScoped<IUnifiedCacheService<Guid, Company, CompanyModel>, CompanyService>()

            // JobTitle
            .AddScoped<IJobTitleService, JobTitleService>()
            .AddScoped<IUnifiedCacheService<Guid, JobTitle, JobTitleModel>, JobTitleService>()
            .AddScoped<IJobTitleValidation, JobTitleValidationHelper>()
            .AddScoped<IJobTitleSearch, JobTitleSearchHelper>()

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

            // Audit Process
            .AddScoped<IAuditProcessService, AuditProcessService>()
            .AddScoped<IUnifiedCacheService<Guid, AuditProcess, AuditProcessModel>, AuditProcessService>()
            .AddScoped<IAuditProcessValidation, AuditProcessValidationHelper>()
            .AddScoped<IAuditProcessSearch, AuditProcessSearchHelper>()

            // Sub Process
            .AddScoped<ISubProcessService, SubProcessService>()
            .AddScoped<IUnifiedCacheService<Guid, SubProcess, SubProcessModel>, SubProcessService>()
            .AddScoped<ISubProcessValidation, SubProcessValidationHelper>()
            .AddScoped<ISubProcessSearch, SubProcessSearchHelper>()

            // Risk Control Matrix
            .AddScoped<IRiskControlMatrixService, RiskControlMatrixService>()
            .AddScoped<IUnifiedCacheService<Guid, RiskControlMatrix, RiskControlMatrixModel>, RiskControlMatrixService>()
            .AddScoped<IRiskControlMatrixSearch, RiskControlMatrixSearchHelper>()
            .AddScoped<IRiskControlMatrixValidation, RiskControlMatrixValidationHelper>()

            // Objectives
            .AddScoped<IObjectiveService, ObjectiveService>()
            .AddScoped<IUnifiedCacheService<Guid, Objective, ObjectiveModel>, ObjectiveService>()
            .AddScoped<IObjectiveValidation, ObjectiveValidationHelper>()
            .AddScoped<IObjectiveSearch, ObjectiveSearchHelper>()

            // Risks
            .AddScoped<IRiskService, RiskService>()
            .AddScoped<IUnifiedCacheService<Guid, Risk, RiskModel>, RiskService>()
            .AddScoped<IRiskValidation, RiskValidationHelper>()
            .AddScoped<IRiskSearch, RiskSearchHelper>()

            // Exit Controls
            .AddScoped<IExitControlService, ExitControlService>()
            .AddScoped<IUnifiedCacheService<Guid, ExitControl, ExitControlModel>, ExitControlService>()
            .AddScoped<IExitControlValidation, ExitControlValidationHelper>()
            .AddScoped<IExitControlSearch, ExitControlSearchHelper>()

            // Control Designs
            .AddScoped<IControlDesignService, ControlDesignService>()
            .AddScoped<IUnifiedCacheService<Guid, ControlDesign, ControlDesignModel>, ControlDesignService>()
            .AddScoped<IControlDesignValidation, ControlDesignValidationHelper>()
            .AddScoped<IControlDesignSearch, ControlDesignSearchHelper>()

            // Observations
            .AddScoped<IObservationService, ObservationService>()
            .AddScoped<IUnifiedCacheService<Guid, Observation, ObservationModel>, ObservationService>()
            .AddScoped<IObservationValidation, ObservationValidationHelper>()
            .AddScoped<IObservationSearch, ObservationSearchHelper>()

            // Recommendations
            .AddScoped<IRecommendationService, RecommendationService>()
            .AddScoped<IUnifiedCacheService<Guid, Recommendation, RecommendationModel>, RecommendationService>()
            .AddScoped<IRecommendationValidation, RecommendationValidationHelper>()
            .AddScoped<IRecommendationSearch, RecommendationSearchHelper>()

            // Supporting Docs
            .AddScoped<ISupportingDocService, SupportingDocService>()
            .AddScoped<IUnifiedCacheService<Guid, SupportingDoc, SupportingDocModel>, SupportingDocService>()
            .AddScoped<ISupportingDocValidation, SupportingDocValidationHelper>()
            .AddScoped<ISupportingDocSearch, SupportingDocSearchHelper>()

            // File Path Builder
            .AddSingleton<IFilePathBuilder, FilePathBuilder>()

            // Url
            .AddSingleton<ISignedUrlService, SignedUrlService>()
            .AddScoped<IURLService, URLService>()

            // Download Service
            .AddScoped<IDecodeDownloadService, DecodeDownloadService>()

            // User Service
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserSessionManager, UserSessionManager>();


        return services;
    }

    private static IServiceCollection AddMappers(this IServiceCollection services) =>
        services.AddAutoMapper(
            typeof(BusinessLogicDependencies).Assembly,
            typeof(AuditMappingProfile).Assembly,
            typeof(CommonMappingProfile).Assembly,
            typeof(OrganizationMappingProfile).Assembly,
            typeof(ProcessMappingProfile).Assembly,
            typeof(RiskControlsMappingProfile).Assembly,
            typeof(RisksMappingProfile).Assembly,
            typeof(SupportingDocsMappingProfile).Assembly
        );
}