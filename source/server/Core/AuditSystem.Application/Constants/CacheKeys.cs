namespace AuditSystem.Application.Constants;

public static class CacheKeys
{
    public const string CacheKey = "{0}-{1}-{2}"; //0 - prefix; 1 - resource id; 2 - language
    public const string SearchCacheKey = "{0}_search_{1}_page_{2}_size_{3}"; // 0-prefix, 1-term, 2-page, 3-size
    public const string SearchCountCacheKey = "{0}_search_count_{1}"; // NEW → 0: prefix, 1: search term
    
    // User-specific search cache keys
    public const string UserSearchCacheKey = "{0}_user_{1}_search_{2}_page_{3}_size_{4}"; // 0-prefix, 1-userId, 2-term, 3-page, 4-size
    public const string UserSearchCountCacheKey = "{0}_user_{1}_search_count_{2}"; // 0-prefix, 1-userId, 2-term

    //Audit
    public const string AuditProgram = "audit-program";
    
    //Common
    public const string Rate = "rate";

    //Organization
    public const string Company = "company";
    public const string Department = "department";
    public const string Location = "location";
    public const string SubDepartment = "sub-department";
    public const string SubLocation = "sub-location";

    //Processes
    public const string AuditProcess = "audit-process";
    public const string SubProcess = "sub-process";

    //RiskControl
    public const string RiskControlMatrix = "risk-control-matrix";
    public const string Objective = "objective";

    //Risk
    public const string Risk = "risk";
    public const string ExitControl = "exit-control";

    //SupportingDocs
    public const string SupportingDoc = "supporting-doc";
}