using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Risks;


namespace AuditSystem.Application.Constants.Risks.RiskConst;

public static class RiskConstants
{   
    public static readonly string[] RiskTags = ["risks", "risk-list"];
    public static readonly string[] ListTags = ["risk-list"];
    public static readonly Expression<Func<Risk, object?>>[] IncludeHierarchy =
    [
        r => r.Objective,
        r => r.Objective.RiskControlMatrix,
        r => r.Objective.RiskControlMatrix.SubProcess,
        r => r.Objective.RiskControlMatrix.SubProcess.AuditProcess,
        r => r.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment,
        r => r.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department,
        r => r.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation,
        r => r.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location,
    ];

    public const string EntityName = "Risk";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access risk {1}.";
    public const string UnauthorizedObjectiveAccessMessage = "User {0} is not authorized to access objective {1}.";
    public const string RiskNotFoundMessage = "Risk with ID {0} not found.";
    public const string ObjectiveNotFoundMessage = "Objective with ID {0} not found.";
    public const string UnauthorizedRiskAccessMessage = "User {0} is not authorized to access risk {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved risk search results from cache";
}