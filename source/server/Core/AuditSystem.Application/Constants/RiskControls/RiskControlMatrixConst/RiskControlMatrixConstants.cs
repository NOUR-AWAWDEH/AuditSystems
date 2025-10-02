using System.Linq.Expressions;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;

public static class RiskControlMatrixConstants
{   
    public static readonly string[] RiskControlMatrixTags = ["risk-control-matrices", "risk-control-matrix-list"];
    public static readonly string[] ListTags = ["risk-control-matrix-list"];
    
    public static readonly Expression<Func<RiskControlMatrix, Guid>> RiskControlMatrixOwnerIdSelector =
        sp => sp.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId;
    
    public const string EntityName = "RiskControlMatrix";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access risk control matrix {1}.";
    public const string UnauthorizedRiskControlMatrixCreateMessage = "User {0} is not authorized to create risk control matrix under SubDepartment {1}.";
    public const string RiskControlMatrixNotFoundMessage = "RiskControlMatrix with ID {0} not found.";
    public const string SubProcessNotFoundMessage = "SubProcess with ID {0} not found.";
    public const string UnauthorizedRiskControlMatrixAccessMessage = "User {0} is not authorized to access risk control matrix {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved risk control matrix search results from cache";
}