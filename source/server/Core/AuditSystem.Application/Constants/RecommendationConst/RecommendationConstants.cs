using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Recommendations;

namespace AuditSystem.Application.Constants.RecommendationConst;

public static class RecommendationConstants
{
    public static readonly string[] RecommendationTags = ["recommendation", "recommendation-list"];
    public static readonly string[] ListTags = ["recommendation-list"];

    public static readonly Expression<Func<Recommendation, Guid>> RecommendationOwnerIdSelector =
        o => o.CreatedByUserId;
    
    public static Expression<Func<Recommendation, bool>> RecommendationOwnerPredicate(Guid userId) =>
        o => o.CreatedByUserId == userId;
    
    public const string EntityName = "Recommendation";
    
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access recommendation {1}.";
    public const string UnauthorizedObservationAccessMessage = "User {0} is not authorized to access observation {1}.";
    public const string RecommendationNotFoundMessage = "Recommendation with ID {0} not found.";
    public const string ObservationNotFoundMessage = "Observation with ID {0} not found.";
    public const string UnauthorizedRecommendationAccessMessage = "User {0} is not authorized to access recommendation with id: {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedRecommendationSearchResultsFromCacheMessage = "Retrieved recommendations search results from cache";
}