using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Application.Constants.Organization.SubDepartmentConst;

public static class SubDepartmentConstants
{
    public static readonly string[] SubDepartmentTags = ["sub-departments", "sub-department-list"];
    public static readonly string[] ListTags = ["sub-department-list"];
    public static readonly Expression<Func<SubDepartment, object?>>[] IncludeDepartment = [sl => sl.Department];
    
    public static readonly Expression<Func<SubDepartment, Guid>> SubDepartmentOwnerIdSelector =
        s => s.CreatedByUserId;
    
    public static Expression<Func<SubDepartment, bool>> SubDepartmentOwnerPredicate(Guid userId) =>
           s => s.CreatedByUserId == userId;
        
    public const string EntityName = "SubDepartment";
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access subdepartment {1}.";
    public const string DepartmentNotFoundMessage = "Department with ID {0} not found.";
    public const string SubDepartmentNotFoundMessage = "SubDepartment with ID {0} not found.";
    public const string UnauthorizedLocationAccessMessage = "User {0} is not authorized to create subdepartment under department {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved search results from cache";
}