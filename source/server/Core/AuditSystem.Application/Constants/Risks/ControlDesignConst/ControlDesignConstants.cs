using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Application.Constants.Risks.ControlDesignConst;

public static class ControlDesignConstants
{
    public static readonly string[] ControlDesignTags = ["control-designs", "control-design-list"];
    public static readonly string[] ListTags = ["control-design-list"];
    
    public static readonly Expression<Func<ControlDesign, Guid>> ControlDesignOwnerIdSelector = 
        cd => cd.ExitControl.Objective.RiskControlMatrix.SubProcess
                .AuditProcess.SubDepartment.Department.Location.UserId;
    
    public static Expression<Func<ControlDesign, bool>> ControlDesignOwnerPredicate(Guid userId) =>
        cd => cd.ExitControl.Objective.RiskControlMatrix.SubProcess
                .AuditProcess.SubDepartment.Department.Location.UserId == userId;
        
    public const string EntityName = "ControlDesign";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access control design {1}.";
    public const string UnauthorizedExitControlAccessMessage = "User {0} is not authorized to access exit control {1}.";
    public const string ControlDesignNotFoundMessage = "Control Design with ID {0} not found.";
    public const string ExitControlNotFoundMessage = "Exit Control with ID {0} not found.";
    public const string UnauthorizedControlDesignAccessMessage = "User {0} is not authorized to access control design {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved exit control search results from cache";
}