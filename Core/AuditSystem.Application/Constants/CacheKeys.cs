namespace AuditSystem.Application.Constants;

public static class CacheKeys
{
    public const string CacheKey = "{0}-{1}-{2}"; //0 - prefix; 1 - resource id; 2 - language
    
    //Audit
    public const string AuditDomain = "AuditDomain";
    public const string AuditEngagement = "AuditEngagement";
    public const string AuditPlanSummary = "AuditPlanSummary";
    public const string AuditUniverseObjective = "AuditUniverseObjective";
    public const string AuditUniverse = "AuditUniverse";
    public const string BusinessObjective = "BusinessObjective";
    public const string SpecialProject = "SpecialProject";

    //Checklist
    public const string ChecklistManagement = "ChecklistManagement";
    public const string Checklist = "Checklist";
    public const string Remark = "Remark";

    //Common
    public const string Rating = "Rating";

    //Compliance
    public const string ComplianceAuditLink = "ComplianceAuditLink";
    public const string ComplianceChecklist = "ComplianceChecklist";

    //jobs
    public const string AuditJob = "AuditJob";
    public const string JobPrioritization = "JobPrioritization";
    public const string JobScheduling = "JobScheduling";

    //Organization
    public const string Company = "Company";
    public const string Department = "Department";
    public const string Location = "Location";
    public const string SubDepartment = "SubDepartment";
    public const string SubLocation = "SubLocation";

    //Processes
    public const string AuditProcess = "AuditProcess";
    public const string SubProcess = "SubProcess";

    //Reports
    public const string AuditExceptionReport = "AuditExceptionReport";
    public const string AuditPlanSummaryReport = "AuditPlanSummaryReport";
    public const string InternalAuditConsolidationReport = "InternalAuditConsolidationReport";
    public const string JobTimeAllocationReport = "JobTimeAllocationReport";
    public const string PlanningReport = "PlanningReport";
    public const string ReportingFollowUpReport = "ReportingFollowUpReport";

    //RiskControl
    public const string RiskControlMatrix = "RiskControlMatrix";
    public const string RiskControl = "RiskControl";
    public const string RiskProgram = "RiskProgram";

    //Risk
    public const string RiskAssessment = "RiskAssessment";
    public const string RiskFactor = "RiskFactor";
    public const string Risk = "Risk";
    public const string SpecialRiskFactor = "SpecialRiskFactor";

    //Skills
    public const string Skill = "Skill";
    public const string SkillSet = "SkillSet";

    //SupportingDocs
    public const string SupportingDoc = "SupportingDoc";

    //Tasks
    public const string TaskManagement = "TaskManagement";

    //User
    public const string UserManagement = "UserManagement";
}