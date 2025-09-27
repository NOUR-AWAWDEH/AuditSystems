using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Application.Constants.Risks.RiskConst;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.BusinessLogic.Helpers.ExitControlHelpers;

public class ExitControlValidationHelper(IRepository<Guid, ExitControl> repository) : IExitControlValidation
{
    private readonly IRepository<Guid, ExitControl> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(ExitControlConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId)
    {
        var exitControl = await _repository.GetByIdAsync(exitControlId, ExitControlConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(ExitControlConstants.ExitControlNotFoundMessage, exitControlId));
        
        if (exitControl.Risk?.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, exitControlId));

        return exitControl;
    }

    public async Task<Risk> ValidateRiskOwnershipAsync(Guid riskId, Guid userId)
    {
        var risk = await _repository.GetByIdAsync(riskId, RiskConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(RiskConstants.RiskNotFoundMessage, riskId));
        
        if (risk.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(RiskConstants.UnauthorizedAccessMessage, userId, riskId));
        
        return risk;
    }
}