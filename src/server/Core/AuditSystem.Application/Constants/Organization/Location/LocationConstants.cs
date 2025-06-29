using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;
using LocationEntity = AuditSystem.Domain.Entities.Organization.Location;
namespace AuditSystem.Application.Constants.Organization.Location;

public static class LocationConstants
{
    public static readonly string[] LocationTags = ["locations", "location-list"];
    public static readonly string[] ListTags = ["location-list"];
    public static readonly Expression<Func<LocationEntity, object?>>[] IncludeCompany = [l => l.Company];
    public static readonly Expression<Func<LocationEntity, object?>>[] IncludeUser = [l => l.User];
    public static readonly Expression<Func<LocationEntity, object?>>[] IncludeAll = [l => l.Company, l => l.User];
    
    public const string EntityName = "Location";
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access location {1}.";
    public const string LocationNotFoundMessage = "Location with ID {0} not found.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string UnauthorizedLocationAccessMessage = "User {0} is not authorized to access location {1}.";
}