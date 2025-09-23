namespace AuditSystem.Application.Constants;

public static class CacheKeys
{
    public const string CacheKey = "{0}-{1}-{2}"; //0 - prefix; 1 - resource id; 2 - language
    public const string SearchCacheKey = "{0}_search_{1}_page_{2}_size_{3}"; // 0-prefix, 1-term, 2-page, 3-size
    public const string SearchCountCacheKey = "{0}_search_count_{1}"; 
    
    public const string UserSearchCacheKey = "{0}_search_user_{1}_{2}_{3}_{4}_{5}"; // 0-prefix, 1-userId, 2-term, 3-relatedId, 4-page, 5-size
    public const string UserSearchCountCacheKey = "{0}_search_count_user_{1}_{2}_{3}"; 

    public const string UserListCacheKey = "{0}_list_user_{1}";

    public const string AuditProgram = "auditprogram";
    
    //Common
    public const string Rate = "rate";

    public const string Company = "company";
    public const string Department = "department";
    public const string Location = "location";
    public const string SubDepartment = "subdepartment";
    public const string SubLocation = "sublocation";

    public const string AuditProcess = "auditprocess";
    public const string SubProcess = "subprocess";

    public const string RiskControlMatrix = "riskcontrolmatrix";
    public const string Objective = "objective";

    public const string Risk = "risk";
    public const string ExitControl = "exitcontrol";

    public const string SupportingDoc = "supportingdoc";
}