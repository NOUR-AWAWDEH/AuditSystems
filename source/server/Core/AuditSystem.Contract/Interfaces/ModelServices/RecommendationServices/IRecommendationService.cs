using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Recommendation;

namespace AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;

public interface IRecommendationService
{
    Task<Guid> CreateRecommendationAsync(RecommendationModel recommendationModel, Guid userId);
    Task UpdateRecommendationAsync(RecommendationModel recommendationModel, Guid userId);
    Task DeleteRecommendationAsync(Guid recommendationId, Guid userId);
    Task<RecommendationModel> GetRecommendationByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<RecommendationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid observationId, Page? page);
    Task<IReadOnlyList<RecommendationModel>> GetRecommendationsByUserIdAsync(Guid userId);

    Task<PaginationResult<RecommendationModel>> SearchAsync(string? searchTerm, Page page);
}