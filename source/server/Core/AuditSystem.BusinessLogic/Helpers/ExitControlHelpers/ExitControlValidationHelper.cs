using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;

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

        return await OwnershipValidator.ValidateOwnershipAsync<ExitControl, Guid>(
            _repository.Query(),
            ec => ec.Id == exitControlId,
            ExitControlConstants.ExitControlOwnerIdSelector,
            userId,
            string.Format(ExitControlConstants.ExitControlNotFoundMessage, exitControlId),
            string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, exitControlId)
        );
    }

    public async Task<Risk> ValidateRiskOwnershipAsync(Guid riskId, Guid userId)
    {

        var riskWithExitControl = await OwnershipValidator.ValidateOwnershipAsync<ExitControl, Guid>(
            _repository.Query(),
            ec => ec.Risk.Id == riskId,
            ExitControlConstants.ExitControlOwnerIdSelector,
            userId,
            string.Format(ExitControlConstants.RiskNotFoundMessage, riskId),
            string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, riskId)
        );

        return riskWithExitControl.Risk;
    }
}