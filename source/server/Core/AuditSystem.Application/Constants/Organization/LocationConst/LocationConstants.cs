using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Application.Constants.Organization.LocationConst;


public static class LocationConstants
{
    public static readonly string[] LocationTags = ["locations", "location-list"];
    public static readonly string[] ListTags = ["location-list"];
    public static readonly Expression<Func<Location, Guid>> LocationOwnerIdSelector =
        l => l.CreatedByUserId;
    
    public static Expression<Func<Location, bool>> LocationOwnerPredicate(Guid userId) =>
        l => l.CreatedByUserId == userId;
        
    public const string EntityName = "Location";
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access location {1}.";
    public const string LocationNotFoundMessage = "Location with ID {0} not found.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string UnauthorizedLocationAccessMessage = "User {0} is not authorized to access location {1}.";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
}