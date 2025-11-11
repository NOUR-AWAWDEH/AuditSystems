using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.Application.Constants.RecommendationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Recommendations;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.RecommendationHelpers;

public class RecommendationValidationHelper(
    IRepository<Guid, Recommendation> repository,
    IRepository<Guid, Observation> observationRepository)
    : IRecommendationValidation
{
    private readonly IRepository<Guid, Recommendation> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Observation> _observationRepository = observationRepository
        ?? throw new ArgumentNullException(nameof(observationRepository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                AuditProgramConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Recommendation> ValidateRecommendationOwnershipAsync(Guid recommendationId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<Recommendation, Guid>(
            _repository.Query(),
            o => o.Id == recommendationId,
            RecommendationConstants.RecommendationOwnerIdSelector,
            userId,
            string.Format(RecommendationConstants.RecommendationNotFoundMessage, recommendationId),
            string.Format(RecommendationConstants.UnauthorizedRecommendationAccessMessage, userId, recommendationId)
        );
    }

    public async Task<Observation> ValidateObservationOwnershipAsync(Guid observationId, Guid userId)
    {
        var observation = await _observationRepository.GetByIdAsync(observationId)
             ?? throw new ArgumentException(ObservationConstants.ObservationNotFoundMessage, nameof(observationId));

        var ownerId = await _observationRepository.Query()
            .AsNoTracking()
            .Where(a => a.Id == observationId)  
            .Select(ObservationConstants.ObservationOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(ObservationConstants.UnauthorizedObservationAccessMessage, userId, observationId)
            );

        return observation;
    }
}