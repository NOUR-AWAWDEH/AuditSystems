using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;

namespace AuditSystem.Application.Constants.ObservationConst;

public static class ObservationConstants
{
    public static readonly string[] ObservationTags = ["observation", "observation-list"];
    public static readonly string[] ListTags = ["observation-list"];

    public static readonly Expression<Func<Observation, Guid>> ObservationOwnerIdSelector =
        o => o.CreatedByUserId;
    
    public static Expression<Func<Observation, bool>> ObservationOwnerPredicate(Guid userId) =>
        o => o.CreatedByUserId == userId;
    
    public const string EntityName = "Observation";
    
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access observation {1}.";
    public const string UnauthorizedAuditProgramAccessMessage = "User {0} is not authorized to access audit program {1}.";
    public const string ObservationNotFoundMessage = "Observation with ID {0} not found.";
    public const string AuditProgramNotFoundMessage = "Audit program with ID {0} not found.";
    public const string UnauthorizedObservationAccessMessage = "User {0} is not authorized to access observation with id: {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved observations search results from cache";
}