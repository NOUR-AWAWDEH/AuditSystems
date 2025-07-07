using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Application.Constants.Organization.DepartmentConst;

public static class DepartmentConstants
{
    public static readonly string[] DepartmentTags = ["department", "department-list"];
    public static readonly string[] ListTags = ["department-list"];
    public static readonly Expression<Func<Department, object?>>[] IncludeSubLocation = [d => d.SubLocation];    
    public const string EntityName = "Department";
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access department {1}.";
    public const string DepartmentNotFoundMessage = "Department with ID {0} not found.";
    public const string SubLocationNotFoundMessage = "SubLocation with ID {0} not found.";
    public const string UnauthorizedDepartmentAccessMessage = "User {0} is not authorized to create department under location {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
}
