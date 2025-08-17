using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Application.Constants.Risks.RiskConst;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.BusinessLogic.Helpers.RiskHelpers;

public class RiskValidationHelper : IRiskValidation
{
    private readonly IRepository<Guid, Risk> _repository;
    public RiskValidationHelper(IRepository<Guid, Risk> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(RiskConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Risk> ValidateRiskOwnershipAsync(Guid riskId, Guid userId)
    {
        var risk = await _repository.GetByIdAsync(riskId, RiskConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(RiskConstants.RiskNotFoundMessage, riskId));
        
        if (risk.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(RiskConstants.UnauthorizedAccessMessage, userId, riskId));

        return risk;
    }

    public async Task<Objective> ValidateObjectiveOwnershipAsync(Guid objectiveId, Guid userId)
    {
        var objective = await _repository.GetByIdAsync(objectiveId, ObjectiveConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(ObjectiveConstants.ObjectiveNotFoundMessage, objectiveId));
        
        if (objective.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(ObjectiveConstants.UnauthorizedAccessMessage, userId, objectiveId));
        
        return objective;
    }
}