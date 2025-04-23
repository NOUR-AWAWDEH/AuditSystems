using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.CommonServices;

public interface IRatingService
{
    public Task<Guid> CreateRatingAsync(RatingModel ratingModel);
    public Task UpdateRatingAsync(RatingModel ratingModel);
    public Task DeleteRatingAsync(Guid ratingId);
    public Task<RatingModel> GetRatingByIdAsync(Guid id);
    public Task<PaginationResult<RatingModel>> SearchAsync(string? name, Page page);
}
