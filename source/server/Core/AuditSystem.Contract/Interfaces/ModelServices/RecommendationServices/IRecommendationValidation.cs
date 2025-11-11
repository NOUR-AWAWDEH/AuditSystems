using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Recommendations;

namespace AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;

public interface IRecommendationValidation
{
     Task<Recommendation> ValidateRecommendationOwnershipAsync(Guid recommendationId, Guid userId);
     Task<Observation> ValidateObservationOwnershipAsync(Guid observationId, Guid userId);
}