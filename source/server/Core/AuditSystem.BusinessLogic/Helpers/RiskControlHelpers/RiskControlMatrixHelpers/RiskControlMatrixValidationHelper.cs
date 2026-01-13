using AuditSystem.Application.Constants.Processes.AuditProcessConst;
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
    IRepository<Guid, AuditProcess> auditProcessRepository) 
    : IRiskControlMatrixValidation
{
    private readonly IRepository<Guid, RiskControlMatrix> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, AuditProcess> _auditProcessRepository = auditProcessRepository ?? throw new ArgumentNullException(nameof(auditProcessRepository));
    
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

    public async Task<AuditProcess> ValidateAuditProcessOwnershipAsync(Guid auditProcessId, Guid userId)
    {
       var auditProcess = await _auditProcessRepository.GetByIdAsync(auditProcessId)
            ?? throw new ArgumentException(string.Format(AuditProcessConstants.AuditProcessNotFoundMessage, auditProcessId), nameof(auditProcessId));

        var ownerId = await _auditProcessRepository.Query()
            .AsNoTracking()
            .Where(ap => ap.Id == auditProcessId)
            .Select(AuditProcessConstants.AuditProcessOwnerIdSelector)
            .FirstOrDefaultAsync();
      
        if (ownerId == Guid.Empty || ownerId != userId)
            throw new ArgumentException(
                string.Format(AuditProcessConstants.UnauthorizedAccessMessage, userId, auditProcessId)
            );
        
        return auditProcess;
    }
}