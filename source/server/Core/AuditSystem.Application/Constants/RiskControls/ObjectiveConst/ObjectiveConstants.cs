using System.Linq.Expressions;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Application.Constants.RiskControls.ObjectiveConst;

public static class ObjectiveConstants
{   
    public static readonly string[] ObjectiveTags = ["objectives", "objective-list"];
    public static readonly string[] ListTags = ["objective-list"];
    
    public static readonly Expression<Func<Objective, Guid>> ObjectiveOwnerIdSelector =
        sp => sp.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.Location.UserId;
    
    public static Expression<Func<Objective, bool>> ObjectiveOwnerPredicate(Guid userId) =>
        o => o.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.Location.UserId == userId;
 
    public const string EntityName = "Objective";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access objective {1}.";
    public const string UnauthorizedObjectiveCreateMessage = "User {0} is not authorized to create objective under SubDepartment {1}.";
    public const string ObjectiveNotFoundMessage = "Objective with ID {0} not found.";
    public const string SubProcessNotFoundMessage = "SubProcess with ID {0} not found.";
    public const string UnauthorizedObjectiveAccessMessage = "User {0} is not authorized to access objective {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved objective search results from cache";
}