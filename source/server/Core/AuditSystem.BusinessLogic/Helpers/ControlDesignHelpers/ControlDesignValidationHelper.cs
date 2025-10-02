using AuditSystem.Application.Constants.Risks.ControlDesignConst;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.BusinessLogic.Utilities;

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
        return await OwnershipValidator.ValidateOwnershipAsync<ControlDesign, Guid>(
            _repository.Query(),
            cd => cd.Id == controlDesignId,
            ControlDesignConstants.ControlDesignOwnerIdSelector,
            userId,
            string.Format(ControlDesignConstants.ControlDesignNotFoundMessage, controlDesignId),
            string.Format(ControlDesignConstants.UnauthorizedAccessMessage, userId, controlDesignId)
        );
    }

    public async Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId)
    {
        var controlDesignWithExitControl = await OwnershipValidator.ValidateOwnershipAsync<ControlDesign, Guid>(
            _repository.Query(),
            cd => cd.ExitControl.Id == exitControlId,
            ControlDesignConstants.ControlDesignOwnerIdSelector,
            userId,
            string.Format(ExitControlConstants.ExitControlNotFoundMessage, exitControlId),
            string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, exitControlId)
        );

        return controlDesignWithExitControl.ExitControl;
    }
}