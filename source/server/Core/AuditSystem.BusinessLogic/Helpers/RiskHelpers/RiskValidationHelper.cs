using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Application.Constants.Risks.RiskConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.RiskHelpers;

public class RiskValidationHelper(
    IRepository<Guid, Risk> repository,
    IRepository<Guid, Objective> objectiveRepository) 
    : IRiskValidation
{
    private readonly IRepository<Guid, Risk> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Objective> _objectiveRepository = objectiveRepository ?? throw new ArgumentNullException(nameof(objectiveRepository));

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

    public async Task<Objective> ValidateObjectiveOwnershipAsync(Guid objectiveId, Guid userId)
    {
        var objective = await _objectiveRepository.GetByIdAsync(objectiveId)
            ?? throw new ArgumentException(
                ObjectiveConstants.ObjectiveNotFoundMessage, nameof(objectiveId)
            );
        
        var ownerId = await _objectiveRepository.Query()
            .AsNoTracking()
            .Where(o => o.Id == objectiveId)
            .Select(ObjectiveConstants.ObjectiveOwnerIdSelector)
            .FirstOrDefaultAsync();
        
        if (ownerId == Guid.Empty || ownerId != userId)
            throw new ArgumentException(
                string.Format(ObjectiveConstants.UnauthorizedAccessMessage, userId, objectiveId)
            );
        
        return objective;
    }
}