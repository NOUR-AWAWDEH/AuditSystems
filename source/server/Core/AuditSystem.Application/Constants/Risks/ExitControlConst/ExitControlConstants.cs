using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Risks;


namespace AuditSystem.Application.Constants.Risks.ExitControlConst;

public static class ExitControlConstants
{   
    public static readonly string[] ExitControlTags = ["exit-controls", "exit-control-list"];
    public static readonly string[] ListTags = ["exit-control-list"];
    public static readonly Expression<Func<ExitControl, object?>>[] IncludeHierarchy =
    [
        ec => ec.Risk,
        ec => ec.Risk.Objective,
        ec => ec.Risk.Objective.RiskControlMatrix,
        ec => ec.Risk.Objective.RiskControlMatrix.SubProcess,
        ec => ec.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess,
        ec => ec.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment,
        ec => ec.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department,
        ec => ec.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation,
        ec => ec.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location,
        ec => ec.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.Company,
    ];

    public const string EntityName = "ExitControl";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access exit control {1}.";
    public const string UnauthorizedRiskAccessMessage = "User {0} is not authorized to access risk {1}.";
    public const string ExitControlNotFoundMessage = "ExitControl with ID {0} not found.";
    public const string RiskNotFoundMessage = "Risk with ID {0} not found.";
    public const string UnauthorizedExitControlAccessMessage = "User {0} is not authorized to access exit control {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved exit control search results from cache";
}