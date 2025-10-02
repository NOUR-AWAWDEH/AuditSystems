using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.Application.Constants.Processes.SubProcessConst;

public static class SubProcessConstants
{   
    public static readonly string[] SubProcessTags = ["sub-processes", "sub-process-list"];
    public static readonly string[] ListTags = ["sub-process-list"];
    
    public static readonly Expression<Func<SubProcess, Guid>> SubProcessOwnerIdSelector =
        sp => sp.AuditProcess.SubDepartment.Department.SubLocation.Location.UserId;

    public const string EntityName = "SubProcess";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access sub process {1}.";
    public const string UnauthorizedAuditProcessCreateMessage = "User {0} is not authorized to create audit process under SubDepartment {1}.";
    public const string SubProcessNotFoundMessage = "SubProcess with ID {0} not found.";
    public const string AuditProcessNotFoundMessage = "AuditProcess with ID {0} not found.";
    public const string UnauthorizedSubProcessAccessMessage = "User {0} is not authorized to access sub process {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved sub process search results from cache";
}