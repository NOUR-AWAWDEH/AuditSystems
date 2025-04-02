using AuditSystem.Application.Features.Audit.AuditDomain.Create;
using AuditSystem.Application.Features.Audit.AuditEngagement.Create;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;
using AuditSystem.Application.Features.Audit.AuditUniverse.Create;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;
using AuditSystem.Application.Features.Audit.BusinessObjective.Create;
using AuditSystem.Application.Features.Audit.Objective.Create;
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
using AuditSystem.Application.Features.Processes.AuditProcess.Creat;
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
using AuditSystem.Application.Features.SupportingDocs.Create;
using AuditSystem.Application.Features.Tasks.Create;
using AuditSystem.Application.Features.Users.Skill.Create;
using AuditSystem.Application.Features.Users.SkillSet.Create;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.Host.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Risks
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class RiskController(IMediator mediator) : ApiControllerBase(mediator)
    {
        //Audits
        //Create Audit Domain
        [HttpPost("Create-Audit-Domain")]
        //[Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditDomainCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditDomain([FromBody] CreateAuditDomainCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Audit Engagement
        [HttpPost("Create-Audit-Engagement")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditEngagementCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditEngagement([FromBody] CreateAuditEngagementCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Audit Plan Summary
        [HttpPost("Create-Audit-Plan-Summary")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditPlanSummaryCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditPlanSummary([FromBody] CreateAuditPlanSummaryCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Audit Universe
        [HttpPost("Create-Audit-Universe")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditUniverseCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditUniverse([FromBody] CreateAuditUniverseCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Audit Universe Objective
        [HttpPost("Create-Audit-Universe-Objective")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditUniverseObjectiveCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditUniverseObjective([FromBody] CreateAuditUniverseObjectiveCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Business Objective
        [HttpPost("Create-Business-Objective")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateBusinessObjectiveCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBusinessObjective([FromBody] CreateBusinessObjectiveCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Objective
        [HttpPost("Create-Objective")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateObjectiveCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateObjective([FromBody] CreateObjectiveCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Specilal Project
        [HttpPost("Create-Special-Project")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateSpecialProjectCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSpecialProject([FromBody] CreateSpecialProjectCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Checklists
        //Create Checklist
        [HttpPost("Create-Checklist")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateChecklistCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Remark
        [HttpPost("Create-Remark")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRemarkCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRemark([FromBody] CreateRemarkCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Common
        //Create Rating
        [HttpPost("Create-Rating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRatingCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRating([FromBody] CreateRatingCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Compliance
        //Create Compliance
        [HttpPost("Create-Compliance")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateComplianceChecklistCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCompliance([FromBody] CreateComplianceChecklistCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Jobs
        //Create Audit Job
        [HttpPost("Create-Audit-Job")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditJobCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditJob([FromBody] CreateAuditJobCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Job Prioritization
        [HttpPost("Create-Job-Prioritization")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateJobPrioritizationCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateJobPrioritization([FromBody] CreateJobPrioritizationCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Job Scheduling
        [HttpPost("Create-Job-Scheduling")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateJobSchedulingCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateJobScheduling([FromBody] CreateJobSchedulingCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Organization
        //Create Company
        [HttpPost("Create-Company")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateCompanyCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Department
        [HttpPost("Create-Department")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateDepartmentCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Location
        [HttpPost("Create-Location")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateLocationCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Sub Department
        [HttpPost("Create-Sub-Department")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateSubDepartmentCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSubDepartment([FromBody] CreateSubDepartmentCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Sub Location
        [HttpPost("Create-Sub-Location")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateSubLocationCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSubLocation([FromBody] CreateSubLocationCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Processes
        //Create Audit Process
        [HttpPost("Create-Audit-Process")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditProcessCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditProcess([FromBody] CreateAuditProcessCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Audit Sub Process
        [HttpPost("Create-Audit-Sub-Process")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateSubProcessCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditSubProcess([FromBody] CreateSubProcessCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Reports
        //Create Report
        [HttpPost("Create-Report")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditExceptionReportCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateReport([FromBody] CreateAuditExceptionReportCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Audit Plan Summary Report
        [HttpPost("Create-Audit-Plan-Summary-Report")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateAuditPlanSummaryReportCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAuditPlanSummaryReport([FromBody] CreateAuditPlanSummaryReportCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Internal Audit Consolidated Report
        [HttpPost("Create-Internal-Audit-Consolidated-Report")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateInternalAuditConsolidationReportCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateInternalAuditConsolidatedReport([FromBody] CreateInternalAuditConsolidationReportCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Job Time Allocation Report
        [HttpPost("Create-Job-Time-Allocation-Report")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateJobTimeAllocationReportCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateJobTimeAllocationReport([FromBody] CreateJobTimeAllocationReportCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Planning Report
        [HttpPost("Create-Planning-Report")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreatePlanningReportCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePlanningReport([FromBody] CreatePlanningReportCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Reporting Follow Up
        [HttpPost("Create-Reporting-Follow-Up")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateReportingFollowUpCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateReportingFollowUpReport([FromBody] CreateReportingFollowUpCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Create Risk Controls
        //Create Risk Control Matrix
        [HttpPost("Create-Risk-Control-Matrix")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskControlMatrixCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRiskControlMatrix([FromBody] CreateRiskControlMatrixCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Risk Controls
        [HttpPost("Create-Risk-Controls")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskControlsCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRiskControls([FromBody] CreateRiskControlsCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Risk Program
        [HttpPost("Create-Risk-Program")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskProgramCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRiskProgram([FromBody] CreateRiskProgramCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Risks
        //Craete Risk
        [HttpPost("Create-Risk")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRisk([FromBody] CreateRiskCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Risk Assessment
        [HttpPost("Create-Risk-Assessment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskAssessmentCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRiskAssessment([FromBody] CreateRiskAssessmentCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Risk Factor
        [HttpPost("Create-Risk-Factor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateRiskFactorCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateRiskFactor([FromBody] CreateRiskFactorCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //SupportingDocs
        //Create Supporting Document
        [HttpPost("Create-Supporting-Document")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateSupportingDocCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSupportingDocument([FromBody] CreateSupportingDocCommand command) =>
            await ProcessRequestToActionResultAsync(command);


        //Tasks
        //Create TaskManagement
        [HttpPost("Create-Task-Management")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateTaskManagementCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTaskManagement([FromBody] CreateTaskManagementCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Users
        //Create Skill
        [HttpPost("Create-Skill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateSkillCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillCommand command) =>
            await ProcessRequestToActionResultAsync(command);

        //Create Skill Set
        [HttpPost("Create-Skill-Set")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<CreateSkillSetCommandResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateSkillSet([FromBody] CreateSkillSetCommand command) =>
            await ProcessRequestToActionResultAsync(command);
    }
}