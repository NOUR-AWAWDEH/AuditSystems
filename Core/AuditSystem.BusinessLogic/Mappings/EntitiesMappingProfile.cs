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
        CreateMap<AuditDomainModel, AuditDomain>().ReverseMap();
        CreateMap<AuditEngagementModel, AuditEngagement>().ReverseMap();
        CreateMap<AuditPlanSummaryModel, AuditPlanSummary>().ReverseMap();
        CreateMap<AuditUniverseObjectiveModel, AuditUniverseObjective>().ReverseMap();
        CreateMap<AuditUniverseModel, AuditUniverse>().ReverseMap();
        CreateMap<BusinessObjectiveModel, BusinessObjective>().ReverseMap();
        CreateMap<SpecialProjectModel, SpecialProject>().ReverseMap();

        //Checklist
        CreateMap<ChecklistModel, Checklist>().ReverseMap();
        CreateMap<RemarkModel, Remark>().ReverseMap();

        //Common
        CreateMap<RatingModel, Rating>().ReverseMap();

        //Compliance
        CreateMap<ComplianceChecklistModel, ComplianceChecklist>().ReverseMap();

        //Jobs
        CreateMap<AuditJobModel, AuditJob>().ReverseMap();
        CreateMap<JobPrioritizationModel, JobPrioritization>().ReverseMap();
        CreateMap<JobSchedulingModel, JobScheduling>().ReverseMap();

        //Organization
        CreateMap<CompanyModel, Company>().ReverseMap();
        CreateMap<DepartmentModel, Department>().ReverseMap();
        CreateMap<LocationModel, Location>().ReverseMap();
        CreateMap<SubDepartmentModel, SubDepartment>().ReverseMap();
        CreateMap<SubLocationModel, SubLocation>().ReverseMap();

        //Processes
        CreateMap<AuditProcessModel, AuditProcess>().ReverseMap();
        CreateMap<SubProcessModel, SubProcess>().ReverseMap();

        //Reports
        CreateMap<AuditExceptionReportModel, AuditExceptionReport>().ReverseMap();
        CreateMap<AuditPlanSummaryReportModel, AuditPlanSummaryReportModel>().ReverseMap();
        CreateMap<InternalAuditConsolidationReportModel, InternalAuditConsolidationReport>().ReverseMap();
        CreateMap<JobTimeAllocationReportModel, JobTimeAllocationReport>().ReverseMap();
        CreateMap<PlanningReportModel, PlanningReport>().ReverseMap();
        CreateMap<ReportingFollowUpModel, ReportingFollowUpModel>().ReverseMap();

        //RiskControl
        CreateMap<RiskControlMatrixModel, RiskControlMatrix>().ReverseMap();
        CreateMap<RiskControlsModel, RiskControls>().ReverseMap();
        CreateMap<RiskProgramModel, RiskProgram>().ReverseMap();

        //Risks
        CreateMap<RiskAssessmentModel, RiskAssessment>().ReverseMap();
        CreateMap<RiskFactorModel, RiskFactor>().ReverseMap();
        CreateMap<RiskModel, Risk>().ReverseMap();

        //Skills
        CreateMap<SkillModel, Skill>().ReverseMap();
        CreateMap<SkillSetModel, SkillSet>().ReverseMap();

        //SupporitngDoc
        CreateMap<SupportingDocModel, SupportingDoc>().ReverseMap();

        //Tasks
        CreateMap<TaskManagementModel, TaskManagement>().ReverseMap();

        //Users
        CreateMap<UserManagementModel, UserManagement>().ReverseMap();
    }
}