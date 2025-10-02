using AuditSystem.Application.Constants.Processes.AuditProcessConst;
using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.BusinessLogic.Helpers.SubProcessHelpers;

public class SubProcessValidationHelper(IRepository<Guid, SubProcess> repository) : ISubProcessValidation
{
    private readonly IRepository<Guid, SubProcess> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(SubProcessConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<SubProcess> ValidateSubProcessOwnershipAsync(Guid subProcessId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<SubProcess, Guid>(
            _repository.Query(),
            sp => sp.Id == subProcessId,
            SubProcessConstants.SubProcessOwnerIdSelector,
            userId,
            string.Format(SubProcessConstants.SubProcessNotFoundMessage, subProcessId),
            string.Format(SubProcessConstants.UnauthorizedAccessMessage, userId, subProcessId)
        );
    }

    public async Task<AuditProcess> ValidateAuditProcessOwnershipAsync(Guid auditProcessId, Guid userId)
    {
        var auditProcessWithSubProcess = await OwnershipValidator.ValidateOwnershipAsync<SubProcess, Guid>(
            _repository.Query(),
            sp => sp.AuditProcess.Id == auditProcessId,
            SubProcessConstants.SubProcessOwnerIdSelector,
            userId,
            string.Format(SubProcessConstants.AuditProcessNotFoundMessage, auditProcessId),
            string.Format(SubProcessConstants.UnauthorizedAccessMessage, userId, auditProcessId)
        );
        
        return auditProcessWithSubProcess.AuditProcess;
    }
}