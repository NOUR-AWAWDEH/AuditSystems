using AuditSystem.Application.Constants.Risks.ControlDesignConst;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.BusinessLogic.Helpers.ControlDesignHelpers;

public class ControlDesignValidationHelper(
        IRepository<Guid, ControlDesign> repository)
        : IControlDesignValidation
{
    private readonly IRepository<Guid, ControlDesign> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(ControlDesignConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<ControlDesign> ValidateControlDesignOwnershipAsync(Guid controlDesignId, Guid userId)
    {
        var controlDesign = await _repository.GetByIdAsync(controlDesignId, ControlDesignConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(ControlDesignConstants.ControlDesignNotFoundMessage, controlDesignId));

        if (controlDesign.ExitControl?.Risk?.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(ControlDesignConstants.UnauthorizedAccessMessage, userId, controlDesignId));

        return controlDesign;
    }

    public async Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId)
    {
        var exitControl = await _repository.GetByIdAsync(exitControlId, ExitControlConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(ExitControlConstants.ExitControlNotFoundMessage, exitControlId));

        if (exitControl.Risk?.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, exitControlId));

        return exitControl;
    }
}