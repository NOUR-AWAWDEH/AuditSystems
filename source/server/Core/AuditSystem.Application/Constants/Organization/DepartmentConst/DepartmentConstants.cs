using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Application.Constants.Organization.DepartmentConst;

public static class DepartmentConstants
{
    public static readonly string[] DepartmentTags = ["department", "department-list"];
    public static readonly string[] ListTags = ["department-list"];
    public static readonly Expression<Func<Department, object?>>[] IncludeHierarchy =
    [
        d => d.SubLocation,
        d => d.SubLocation.Location
    ];


    public const string EntityName = "Department";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access department {1}.";
    public const string UnauthorizedSubLocationAccessMessage = "User {0} is not authorized to create department under subLocation {1}.";
    public const string DepartmentNotFoundMessage = "Department with ID {0} not found.";
    public const string SubLocationNotFoundMessage = "SubLocation with ID {0} not found.";
    public const string UnauthorizedDepartmentAccessMessage = "User {0} is not authorized to create department under location {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved departments search results from cache";
}
