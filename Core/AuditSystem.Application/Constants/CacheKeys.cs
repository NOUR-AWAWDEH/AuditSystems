namespace AuditSystem.Application.Constants;

public static class CacheKeys
{
    public const string CacheKey = "{0}-{1}-{2}"; //0 - prefix; 1 - resource id; 2 - language
    
    //Audit
    public const string AuditDomain = "audit-domain";
    public const string AuditEngagement = "audit-engagement";
    public const string AuditPlanSummary = "audit-plan-summary";
    public const string AuditUniverseObjective = "audit-universe-objective";
    public const string AuditUniverse = "audit-universe";
    public const string BusinessObjective = "business-objective";
    public const string SpecialProject = "special-project";

    //Checklist
    public const string ChecklistManagement = "checklist-management";
    public const string Checklist = "checklist";
    public const string Remark = "remark";

    //Common
    public const string Rating = "rating";

    //Compliance
    public const string ComplianceAuditLink = "compliance-auditLink";
    public const string ComplianceChecklist = "compliance-checklist";

    //jobs
    public const string AuditJob = "audit-job";
    public const string JobPrioritization = "job-prioritization";
    public const string JobScheduling = "job-scheduling";

    //Organization
    public const string Company = "company";
    public const string Department = "department";
    public const string Location = "location";
    public const string SubDepartment = "sub-department";
    public const string SubLocation = "sub-location";

    //Processes
    public const string AuditProcess = "audit-process";
    public const string SubProcess = "sub-process";

    //Reports
    public const string AuditExceptionReport = "audit-exception-report";
    public const string AuditPlanSummaryReport = "audit-plan-summary-report";
    public const string InternalAuditConsolidationReport = "internal-audit-consolidation-report";
    public const string JobTimeAllocationReport = "job-time-allocation-report";
    public const string PlanningReport = "planning-report";
    public const string ReportingFollowUp = "reporting-follow-up";

    //RiskControl
    public const string RiskControlMatrix = "risk-control-matrix";
    public const string RiskControl = "risk-control";
    public const string RiskProgram = "risk-program";

    //Risk
    public const string RiskAssessment = "risk-assessment";
    public const string RiskFactor = "risk-factor";
    public const string Risk = "risk";
    public const string SpecificRiskFactor = "special-risk-factor";

    //Skills
    public const string Skill = "skill";
    public const string SkillSet = "skill-set";

    //SupportingDocs
    public const string SupportingDoc = "supporting-doc";

    //Tasks
    public const string TaskManagement = "task-management";

    //User
    public const string UserManagement = "user-management";
}