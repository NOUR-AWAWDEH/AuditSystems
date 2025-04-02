using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Interfaces.ModelServices.CommonServices;

public interface IRatingService
{
    public Task<Guid> CreateRatingAsync(RatingModel ratingModel);
}
