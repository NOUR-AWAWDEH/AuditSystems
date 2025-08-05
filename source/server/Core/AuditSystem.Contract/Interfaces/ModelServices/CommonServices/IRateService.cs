using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.CommonServices;

public interface IRateService
{
    public Task<Guid> CreateRatingAsync(RateModel ratingModel);
    public Task UpdateRatingAsync(RateModel ratingModel);
    public Task DeleteRatingAsync(Guid ratingId);
    public Task<RateModel> GetRatingByIdAsync(Guid id);
    public Task<PaginationResult<RateModel>> SearchAsync(string? name, Page page);
}
