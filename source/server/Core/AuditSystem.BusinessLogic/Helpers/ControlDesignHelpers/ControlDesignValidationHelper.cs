using AuditSystem.Application.Constants.Risks.ControlDesignConst;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.BusinessLogic.Utilities;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.ControlDesignHelpers;

public class ControlDesignValidationHelper(
    IRepository<Guid, ControlDesign> repository,
    IRepository<Guid, ExitControl> exitControlRepository)
    : IControlDesignValidation
{
    private readonly IRepository<Guid, ControlDesign> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, ExitControl> _exitControlRepository = exitControlRepository ?? throw new ArgumentNullException(nameof(exitControlRepository));

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
        var exitControl = await _exitControlRepository.GetByIdAsync(exitControlId) 
            ?? throw new ArgumentException(ExitControlConstants.ExitControlNotFoundMessage, nameof(exitControlId));
         
        var ownerId = await _exitControlRepository.Query()
            .AsNoTracking()
            .Where(ec => ec.Id == exitControlId)
            .Select(ExitControlConstants.ExitControlOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, exitControlId)
            );
         
        return exitControl;
    }
}