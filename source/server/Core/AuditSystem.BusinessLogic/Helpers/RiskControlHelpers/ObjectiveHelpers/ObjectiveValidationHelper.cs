using AuditSystem.Application.Constants.RiskControls.ObjectiveConst;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.RiskControls;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.ObjectiveHelpers;

public class ObjectiveValidationHelper(
    IRepository<Guid, Objective> repository,
    IRepository<Guid, RiskControlMatrix> riskControlMatrixRepository) 
    : IObjectiveValidation
{
    private readonly IRepository<Guid, Objective> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, RiskControlMatrix> _riskControlMatrixRepository = riskControlMatrixRepository ?? throw new ArgumentNullException(nameof(riskControlMatrixRepository));

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
       var riskControlMatrix = await _riskControlMatrixRepository.GetByIdAsync(riskControlMatrixId)
            ?? throw new ArgumentException(RiskControlMatrixConstants.RiskControlMatrixNotFoundMessage, nameof(riskControlMatrixId));
           
       var ownerId = await _riskControlMatrixRepository.Query()
            .AsNoTracking()
            .Where(rcm => rcm.Id == riskControlMatrixId)
            .Select(RiskControlMatrixConstants.RiskControlMatrixOwnerIdSelector)
            .FirstOrDefaultAsync();
            
       if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(RiskControlMatrixConstants.UnauthorizedAccessMessage, userId, riskControlMatrixId)
            );
        
       return riskControlMatrix;
    }
}
