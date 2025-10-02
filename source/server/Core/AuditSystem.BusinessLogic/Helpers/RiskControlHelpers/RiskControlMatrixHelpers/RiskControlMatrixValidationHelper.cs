using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.RiskControlMatrixHelpers;

public class RiskControlMatrixValidationHelper(IRepository<Guid, RiskControlMatrix> repository) : IRiskControlMatrixValidation
{
    private readonly IRepository<Guid, RiskControlMatrix> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

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
       var subProcessWithRiskControlMatrix = await OwnershipValidator.ValidateOwnershipAsync<RiskControlMatrix, Guid>(
            _repository.Query(),
            rcm => rcm.SubProcess.Id == subProcessId,
            RiskControlMatrixConstants.RiskControlMatrixOwnerIdSelector,
            userId,
            string.Format(SubProcessConstants.SubProcessNotFoundMessage, subProcessId),
            string.Format(SubProcessConstants.UnauthorizedAccessMessage, userId, subProcessId)
        );

        return subProcessWithRiskControlMatrix.SubProcess;
    }
}