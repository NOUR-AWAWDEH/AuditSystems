using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Application.Constants.Processes.AuditProcessConst;

public static class AuditProcessConstants
{
    public static readonly string[] AuditProcessTags = ["audit-process", "audit-process-list"];
    public static readonly string[] ListTags = ["audit-process-list"];
    
    public static readonly Expression<Func<AuditProcess, Guid>> AuditProcessOwnerIdSelector =
        ap => ap.SubDepartment.Department.SubLocation.Location.UserId;

    public const string EntityName = "AuditProcess";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access audit process {1}.";
    public const string UnauthorizedSubDepartmentAccessMessage = "User {0} is not authorized to create audit process under SubDepartment {1}.";
    public const string AuditProcessNotFoundMessage = "Audit Process with ID {0} not found.";
    public const string SubDepartmentNotFoundMessage = "SubDepartment with ID {0} not found.";
    public const string UnauthorizedAuditProcessAccessMessage = "User {0} is not authorized to create audit process under department {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved audit processes search results from cache";
}