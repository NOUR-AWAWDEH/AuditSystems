using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.Application.Constants.RiskControls.RiskControlMatrixConst;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.BusinessLogic.Helpers.RiskControlHelpers.RiskControlMatrixHelpers;

public class RiskControlMatrixValidationHelper : IRiskControlMatrixValidation
{
    private readonly IRepository<Guid, RiskControlMatrix> _repository;
    public RiskControlMatrixValidationHelper(IRepository<Guid, RiskControlMatrix> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(RiskControlMatrixConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<RiskControlMatrix> ValidateRiskControlMatrixOwnershipAsync(Guid riskControlMatrixId, Guid userId)
    {
        var riskControlMatrix = await _repository.GetByIdAsync(riskControlMatrixId, RiskControlMatrixConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(RiskControlMatrixConstants.RiskControlMatrixNotFoundMessage, riskControlMatrixId));
        
        if (riskControlMatrix.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(RiskControlMatrixConstants.UnauthorizedAccessMessage, userId, riskControlMatrixId));

        return riskControlMatrix;
    }

    public async Task<SubProcess> ValidateSubProcessOwnershipAsync(Guid subProcessId, Guid userId)
    {
        var subProcess = await _repository.GetByIdAsync(subProcessId, SubProcessConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(SubProcessConstants.SubProcessNotFoundMessage, subProcessId));
        
        if (subProcess.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(SubProcessConstants.UnauthorizedAccessMessage, userId, subProcessId));
        
        return subProcess;
    }
}
