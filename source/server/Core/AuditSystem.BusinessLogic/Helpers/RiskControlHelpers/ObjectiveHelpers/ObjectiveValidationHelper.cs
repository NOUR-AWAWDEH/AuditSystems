using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.ObjectiveHelpers;

public class ObjectiveValidationHelper : IObjectiveValidation
{
    private readonly IRepository<Guid, Objective> _repository;
    public ObjectiveValidationHelper(IRepository<Guid, Objective> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(ObjectiveConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Objective> ValidateObjectiveOwnershipAsync(Guid objectiveId, Guid userId)
    {
        var objective = await _repository.GetByIdAsync(objectiveId, ObjectiveConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(ObjectiveConstants.ObjectiveNotFoundMessage, objectiveId));
        
        if (objective.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(ObjectiveConstants.UnauthorizedAccessMessage, userId, objectiveId));

        return objective;
    }

    public async Task<RiskControlMatrix> ValidateRiskControlMatrixOwnershipAsync(Guid riskControlMatrixId, Guid userId)
    {
        var riskControlMatrix = await _repository.GetByIdAsync(riskControlMatrixId, RiskControlMatrixConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(RiskControlMatrixConstants.RiskControlMatrixNotFoundMessage, riskControlMatrixId));
        
        if (riskControlMatrix.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(RiskControlMatrixConstants.UnauthorizedAccessMessage, userId, riskControlMatrixId));
        
        return riskControlMatrix;
    }
}
