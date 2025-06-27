using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Application.Constants.Organization.Sublocation;

public static class SubLocationConstants
{
    public static readonly string[] SubLocationTags = ["sub-locations", "sub-location-list"];
    public static readonly string[] ListTags = ["sub-location-list"];
    public static readonly Expression<Func<SubLocation, object?>>[] IncludeLocation = [sl => sl.Location];
    
    public const string EntityName = "SubLocation";
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access sublocation {1}.";
    public const string LocationNotFoundMessage = "Location with ID {0} not found.";
    public const string SubLocationNotFoundMessage = "SubLocation with ID {0} not found.";
    public const string UnauthorizedLocationAccessMessage = "User {0} is not authorized to create sublocation under location {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
}