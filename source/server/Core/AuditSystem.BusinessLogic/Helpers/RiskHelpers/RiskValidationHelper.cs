using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Application.Constants.Risks.RiskConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.BusinessLogic.Helpers.RiskHelpers;

public class RiskValidationHelper(IRepository<Guid, Risk> repository) : IRiskValidation
{
    private readonly IRepository<Guid, Risk> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

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
        var objectiveWithRisk = await OwnershipValidator.ValidateOwnershipAsync<Risk, Guid>(
            _repository.Query(),
            rcm => rcm.Objective.Id == objectiveId,
            RiskConstants.RiskOwnerIdSelector,
            userId,
            string.Format(ObjectiveConstants.ObjectiveNotFoundMessage, objectiveId),
            string.Format(ObjectiveConstants.UnauthorizedAccessMessage, userId, objectiveId)
        );
        
        return objectiveWithRisk.Objective;
    }
}