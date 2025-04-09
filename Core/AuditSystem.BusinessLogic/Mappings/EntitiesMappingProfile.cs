using AuditSystem.Application.Features.Audit.AuditDomain.Create;
using AuditSystem.Application.Features.Audit.AuditEngagement.Create;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;
using AuditSystem.Application.Features.Audit.AuditUniverse.Create;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;
using AuditSystem.Application.Features.Audit.BusinessObjective.Create;
using AuditSystem.Application.Features.Audit.SpecialProject.Create;
using AuditSystem.Application.Features.Checklists.Checklist.Create;
using AuditSystem.Application.Features.Checklists.Remark.Create;
using AuditSystem.Application.Features.Common.Rating.Create;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;
using AuditSystem.Application.Features.Jobs.AuditJob.Create;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Create;
using AuditSystem.Application.Features.Jobs.JobScheduling.Create;
using AuditSystem.Application.Features.Organisation.Company.Create;
using AuditSystem.Application.Features.Organisation.Department.Create;
using AuditSystem.Application.Features.Organisation.Location.Create;
using AuditSystem.Application.Features.Organisation.SubDepartment.Create;
using AuditSystem.Application.Features.Organisation.SubLocation.Create;
using AuditSystem.Application.Features.Processes.AuditProcess.Create;
using AuditSystem.Application.Features.Processes.SubProcess.Create;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Create;
using AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Create;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Create;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Create;
using AuditSystem.Application.Features.Reports.PlanningReport.Create;
using AuditSystem.Application.Features.Reports.ReportingFollowUp.Create;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;
using AuditSystem.Application.Features.RiskControls.RiskControls.Create;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Create;
using AuditSystem.Application.Features.Risks.Risk.Create;
using AuditSystem.Application.Features.Risks.RiskAssessment.Create;
using AuditSystem.Application.Features.Risks.RiskFactor.Create;
using AuditSystem.Application.Features.Skills.Skill.Create;
using AuditSystem.Application.Features.Skills.SkillCategory.Create;
using AuditSystem.Application.Features.Skills.SkillSet.Create;
using AuditSystem.Application.Features.SupportingDocs.Create;
using AuditSystem.Application.Features.Tasks.Create;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Contract.Models.Tasks;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Checklists;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Compliance;
using AuditSystem.Domain.Entities.Jobs;
using AuditSystem.Domain.Entities.Organisation;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.Reports;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.Skills;
using AuditSystem.Domain.Entities.SupportingDocs;
using AuditSystem.Domain.Entities.Tasks;
using AuditSystem.Domain.Entities.Users;
using AutoMapper;

namespace IdentityTenantManagement.BusinessLogic.Mappings;

public sealed class EntitiesMappingProfile : Profile
{
    public EntitiesMappingProfile()
    {
        //Audit
        //Domain
        CreateMap<AuditDomainModel, AuditDomain>().ReverseMap();
        CreateMap<CreateAuditDomainCommand, AuditDomainModel>();

        //AuditEngagement
        CreateMap<AuditEngagementModel, AuditEngagement>().ReverseMap();
        CreateMap<CreateAuditEngagementCommand, AuditEngagementModel>();

        //AuditPlanSummary
        CreateMap<AuditPlanSummaryModel, AuditPlanSummary>().ReverseMap();
        CreateMap<CreateAuditPlanSummaryCommand, AuditPlanSummaryModel>();

        //AuditUniverseObjective
        CreateMap<AuditUniverseObjectiveModel, AuditUniverseObjective>().ReverseMap();
        CreateMap<CreateAuditUniverseObjectiveCommand, AuditUniverseObjectiveModel>();

        //AuditUniverse
        CreateMap<AuditUniverseModel, AuditUniverse>().ReverseMap();
        CreateMap<CreateAuditUniverseCommand, AuditUniverseModel>();

        //BusinessObjective
        CreateMap<BusinessObjectiveModel, BusinessObjective>().ReverseMap();
        CreateMap<CreateBusinessObjectiveCommand, BusinessObjectiveModel>();

        //SpecialProject
        CreateMap<SpecialProjectModel, SpecialProject>().ReverseMap();
        CreateMap<CreateSpecialProjectCommand, SpecialProjectModel>();

        //Checklist
        CreateMap<ChecklistModel, Checklist>().ReverseMap();
        CreateMap<CreateChecklistCommand, ChecklistModel>();

        //ChecklistRemark
        CreateMap<RemarkModel, Remark>().ReverseMap();
        CreateMap<CreateRemarkCommand, RemarkModel>();

        //Common
        CreateMap<RatingModel, Rating>().ReverseMap();
        CreateMap<CreateRatingCommand, RatingModel>();

        //Compliance
        CreateMap<ComplianceChecklistModel, ComplianceChecklist>().ReverseMap();
        CreateMap<CreateComplianceChecklistCommand, ComplianceChecklistModel>();

        //Jobs
        CreateMap<AuditJobModel, AuditJob>().ReverseMap();
        CreateMap<CreateAuditJobCommand, AuditJobModel>();

        //JobPrioritization
        CreateMap<JobPrioritizationModel, JobPrioritization>().ReverseMap();
        CreateMap<CreateJobPrioritizationCommand, JobPrioritizationModel>();

        //JobScheduling
        CreateMap<JobSchedulingModel, JobScheduling>().ReverseMap();
        CreateMap<CreateJobSchedulingCommand, JobSchedulingModel>();


        //Organization
        //Company
        CreateMap<CompanyModel, Company>().ReverseMap();
        CreateMap<CreateCompanyCommand, CompanyModel>();

        //Department
        CreateMap<DepartmentModel, Department>().ReverseMap();
        CreateMap<CreateDepartmentCommand, DepartmentModel>();

        //Location
        CreateMap<LocationModel, Location>().ReverseMap();
        CreateMap<CreateLocationCommand, LocationModel>();

        //SubDepartment
        CreateMap<SubDepartmentModel, SubDepartment>().ReverseMap();
        CreateMap<CreateSubDepartmentCommand, SubDepartmentModel>();

        //SubLocation
        CreateMap<SubLocationModel, SubLocation>().ReverseMap();
        CreateMap<CreateSubLocationCommand, SubLocationModel>();


        //Processes
        //AuditProcess
        CreateMap<AuditProcessModel, AuditProcess>().ReverseMap();
        CreateMap<CreateAuditProcessCommand, AuditProcessModel>();

        //SubProcess
        CreateMap<SubProcessModel, SubProcess>().ReverseMap();
        CreateMap<CreateSubProcessCommand, SubProcessModel>();

        //Reports
        CreateMap<AuditExceptionReportModel, AuditExceptionReport>().ReverseMap();
        CreateMap<CreateAuditExceptionReportCommand, AuditExceptionReportModel>();

        //AuditPlanSummaryReport
        CreateMap<AuditPlanSummaryReportModel, AuditPlanSummaryReportModel>().ReverseMap();
        CreateMap<CreateAuditPlanSummaryReportCommand, AuditPlanSummaryReportModel>();

        //InternalAuditConsolidationReport
        CreateMap<InternalAuditConsolidationReportModel, InternalAuditConsolidationReport>().ReverseMap();
        CreateMap<CreateInternalAuditConsolidationReportCommand, InternalAuditConsolidationReportModel>();

        //JobTimeAllocationReport
        CreateMap<JobTimeAllocationReportModel, JobTimeAllocationReport>().ReverseMap();
        CreateMap<CreateJobTimeAllocationReportCommand, JobTimeAllocationReportModel>();

        //PlanningReport
        CreateMap<PlanningReportModel, PlanningReport>().ReverseMap();
        CreateMap<CreatePlanningReportCommand, PlanningReportModel>();

        //AuditExceptionReport
        CreateMap<ReportingFollowUpModel, ReportingFollowUpModel>().ReverseMap();
        CreateMap<CreateReportingFollowUpCommand, ReportingFollowUpModel>();


        //RiskControl
        //RiskControlMatrix
        CreateMap<RiskControlMatrixModel, RiskControlMatrix>().ReverseMap();
        CreateMap<CreateRiskControlMatrixCommand, RiskControlMatrixModel>();

        //RiskControls
        CreateMap<RiskControlsModel, RiskControls>().ReverseMap();
        CreateMap<CreateRiskControlsCommand, RiskControlsModel>();

        //RiskProgram
        CreateMap<RiskProgramModel, RiskProgram>().ReverseMap();
        CreateMap<CreateRiskProgramCommand, RiskProgramModel>();

        //Risks
        //RiskAssessment
        CreateMap<RiskAssessmentModel, RiskAssessment>().ReverseMap();
        CreateMap<CreateRiskAssessmentCommand, RiskAssessmentModel>();

        //RiskAssessment
        CreateMap<RiskFactorModel, RiskFactor>().ReverseMap();
        CreateMap<CreateRiskFactorCommand, RiskFactorModel>();

        //Risk
        CreateMap<RiskModel, Risk>().ReverseMap();
        CreateMap<CreateRiskCommand, RiskModel>();


        //Skills
        //Skill
        CreateMap<SkillModel, Skill>().ReverseMap();
        CreateMap<CreateSkillCommand, SkillModel>();

        //SkillSet
        CreateMap<SkillSetModel, SkillSet>().ReverseMap();
        CreateMap<CreateSkillSetCommand, SkillSetModel>();

        //SkillCategory
        CreateMap<SkillCategoryModel, SkillCategory>().ReverseMap();
        CreateMap<CreateSkillCategoryCommand, SkillCategoryModel>();

        //SupporitngDoc
        //SupportingDoc
        CreateMap<SupportingDocModel, SupportingDoc>().ReverseMap();
        CreateMap<CreateSupportingDocCommand, SupportingDocModel>();


        //Tasks
        //TaskManagement
        CreateMap<TaskManagementModel, TaskManagement>().ReverseMap();
        CreateMap<CreateTaskManagementCommand, TaskManagementModel>();


        //Users
        //UserManagement
        CreateMap<UserManagementModel, UserManagement>().ReverseMap();
        CreateMap<CreateUserManagementCommand, UserManagementModel>();
    }
}