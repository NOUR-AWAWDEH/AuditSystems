using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.RiskControls;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.RiskControlMatrixHelpers;

public class RiskControlMatrixValidationHelper(
    IRepository<Guid, RiskControlMatrix> repository,
    IRepository<Guid, SubProcess> subProcessRepository) 
    : IRiskControlMatrixValidation
{
    private readonly IRepository<Guid, RiskControlMatrix> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, SubProcess> _subProcessRepository = subProcessRepository ?? throw new ArgumentNullException(nameof(subProcessRepository));
    
    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(RiskControlMatrixConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<RiskControlMatrix> ValidateRiskControlMatrixOwnershipAsync(Guid riskControlMatrixId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<RiskControlMatrix, Guid>(
            _repository.Query(),
            rcm => rcm.Id == riskControlMatrixId,
            RiskControlMatrixConstants.RiskControlMatrixOwnerIdSelector,
            userId,
            string.Format(RiskControlMatrixConstants.RiskControlMatrixNotFoundMessage, riskControlMatrixId),
            string.Format(RiskControlMatrixConstants.UnauthorizedAccessMessage, userId, riskControlMatrixId)
        );
    }

    public async Task<SubProcess> ValidateSubProcessOwnershipAsync(Guid subProcessId, Guid userId)
    {
       var subProcess = await _subProcessRepository.GetByIdAsync(subProcessId)
            ?? throw new ArgumentException(SubProcessConstants.SubProcessNotFoundMessage, nameof(subProcessId));

        var ownerId = await _subProcessRepository.Query()
            .AsNoTracking()
            .Where(sp => sp.Id == subProcessId)
            .Select(SubProcessConstants.SubProcessOwnerIdSelector)
            .FirstOrDefaultAsync();
      
        if (ownerId == Guid.Empty || ownerId != userId)
            throw new ArgumentException(
                string.Format(SubProcessConstants.UnauthorizedSubProcessAccessMessage, userId, subProcessId)
            );
        
        return subProcess;
    }
}