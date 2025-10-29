using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;

namespace AuditSystem.BusinessLogic.Helpers.ExitControlHelpers;

public class ExitControlValidationHelper(
    IRepository<Guid, ExitControl> repository, 
    IRepository<Guid, Objective> objectiveRepository) 
    : IExitControlValidation
{
    private readonly IRepository<Guid, ExitControl> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Objective> _objectiveRepository = objectiveRepository ?? throw new ArgumentNullException(nameof(objectiveRepository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(ExitControlConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId)
    {

        return await OwnershipValidator.ValidateOwnershipAsync<ExitControl, Guid>(
            _repository.Query(),
            ec => ec.Id == exitControlId,
            ExitControlConstants.ExitControlOwnerIdSelector,
            userId,
            string.Format(ExitControlConstants.ExitControlNotFoundMessage, exitControlId),
            string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, exitControlId)
        );
    }

    public async Task<Objective> ValidateObjectiveOwnershipAsync(Guid objectiveId, Guid userId)
    {
        var objective = await _objectiveRepository.GetByIdAsync(objectiveId) ?? throw new ArgumentException(ExitControlConstants.ObjectiveNotFoundMessage, nameof(objectiveId));
        
        var ownerId = await _objectiveRepository.Query()
            .AsNoTracking()
            .Where(r => r.Id == objectiveId)
            .Select(ObjectiveConstants.ObjectiveOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, objectiveId)
            );

        return objective;
    }
}