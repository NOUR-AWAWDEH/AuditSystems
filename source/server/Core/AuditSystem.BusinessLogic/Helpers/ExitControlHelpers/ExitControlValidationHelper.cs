using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using Microsoft.EntityFrameworkCore;
using AuditSystem.Application.Constants.Risks.RiskConst;

namespace AuditSystem.BusinessLogic.Helpers.ExitControlHelpers;

public class ExitControlValidationHelper(
    IRepository<Guid, ExitControl> repository, 
    IRepository<Guid, Risk> riskRepository) 
    : IExitControlValidation
{
    private readonly IRepository<Guid, ExitControl> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Risk> _riskRepository = riskRepository ?? throw new ArgumentNullException(nameof(riskRepository));

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
        var risk = await _riskRepository.GetByIdAsync(riskId) ?? throw new ArgumentException(ExitControlConstants.RiskNotFoundMessage, nameof(riskId));
        
        var ownerId = await _riskRepository.Query()
            .AsNoTracking()
            .Where(r => r.Id == riskId)
            .Select(RiskConstants.RiskOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, riskId)
            );

        return risk;
    }
}