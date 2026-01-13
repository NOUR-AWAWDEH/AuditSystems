using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.Application.Constants.Risks.RiskConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.RiskHelpers;

public class RiskValidationHelper(
    IRepository<Guid, Risk> repository,
    IRepository<Guid, Observation> observationRepository) 
    : IRiskValidation
{
    private readonly IRepository<Guid, Risk> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Observation> _observationRepository = observationRepository ?? throw new ArgumentNullException(nameof(observationRepository));

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(RiskConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Risk> ValidateRiskOwnershipAsync(Guid riskId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<Risk, Guid>(
            _repository.Query(),
            r => r.Id == riskId,
            RiskConstants.RiskOwnerIdSelector,
            userId,
            string.Format(RiskConstants.RiskNotFoundMessage, riskId),
            string.Format(RiskConstants.UnauthorizedAccessMessage, userId, riskId)
        );
    }

    public async Task<Observation> ValidateObservationOwnershipAsync(Guid observationId, Guid userId)
    {
        var observation = await _observationRepository.GetByIdAsync(observationId)
            ?? throw new ArgumentException(
                $"Observation with ID {observationId} not found.", nameof(observationId)
            );
        
        var ownerId = await _observationRepository.Query()
            .AsNoTracking()
            .Where(o => o.Id == observationId)
            .Select(ObservationConstants.ObservationOwnerIdSelector)
            .FirstOrDefaultAsync();
        
        if (ownerId == Guid.Empty || ownerId != userId)
            throw new ArgumentException(
                string.Format(ObservationConstants.UnauthorizedAuditProgramAccessMessage, userId, observationId)
            );
        
        return observation;
    }
}