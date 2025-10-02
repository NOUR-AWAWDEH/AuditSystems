using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.ObjectiveHelpers;

public class ObjectiveValidationHelper(IRepository<Guid, Objective> repository) : IObjectiveValidation
{
    private readonly IRepository<Guid, Objective> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(ObjectiveConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Objective> ValidateObjectiveOwnershipAsync(Guid objectiveId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<Objective, Guid>(
            _repository.Query(),
            o => o.Id == objectiveId,
            ObjectiveConstants.ObjectiveOwnerIdSelector,
            userId,
            string.Format(ObjectiveConstants.ObjectiveNotFoundMessage, objectiveId),
            string.Format(ObjectiveConstants.UnauthorizedAccessMessage, userId, objectiveId)
        );
    }

    public async Task<RiskControlMatrix> ValidateRiskControlMatrixOwnershipAsync(Guid riskControlMatrixId, Guid userId)
    {
       var riskControlMatrixWithObjective = await OwnershipValidator.ValidateOwnershipAsync<Objective, Guid>(
            _repository.Query(),
            o => o.RiskControlMatrix.Id == riskControlMatrixId,
           ObjectiveConstants.ObjectiveOwnerIdSelector,
            userId,
            string.Format(RiskControlMatrixConstants.RiskControlMatrixNotFoundMessage, riskControlMatrixId),
            string.Format(RiskControlMatrixConstants.UnauthorizedAccessMessage, userId, riskControlMatrixId)
        );
        
        return riskControlMatrixWithObjective.RiskControlMatrix;
    }
}
