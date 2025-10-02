using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Audit;

namespace AuditSystem.Application.Constants.Audit.AuditProgramConst;

public static class AuditProgramConstants
{
    public static readonly string[] AuditProgramTags = ["audit-program", "audit-program-list"];
    public static readonly string[] ListTags = ["audit-list"];

    public static readonly Expression<Func<AuditProgram, Guid>> AuditProgramOwnerIdSelector =
        a => a.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess
            .AuditProcess.SubDepartment.Department.SubLocation.Location.UserId;

    public const string EntityName = "AuditProgram";    
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access audit program {1}.";
    public const string UnauthorizedExitControlAccessMessage = "User {0} is not authorized to access exit control {1}.";
    public const string AuditProgramNotFoundMessage = "Audit program with ID {0} not found.";
    public const string ExitControlNotFoundMessage = "Exit control with ID {0} not found.";
    public const string UnauthorizedAuditProgramAccessMessage = "User {0} is not authorized to access audit program with id: {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved audit programs search results from cache";
}