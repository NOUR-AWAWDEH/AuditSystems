using AuditSystem.Application.Constants.Processes.AuditProcessConst;
using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;
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
        var subProcess = await _repository.GetByIdAsync(subProcessId, SubProcessConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(SubProcessConstants.SubProcessNotFoundMessage, subProcessId));
        
        if (subProcess.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(SubProcessConstants.UnauthorizedAccessMessage, userId, subProcessId));

        return subProcess;
    }

    public async Task<AuditProcess> ValidateAuditProcessOwnershipAsync(Guid auditProcessId, Guid userId)
    {
        var auditProcess = await _repository.GetByIdAsync(auditProcessId, AuditProcessConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(AuditProcessConstants.AuditProcessNotFoundMessage, auditProcessId));
        
        if (auditProcess.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(AuditProcessConstants.UnauthorizedAccessMessage, userId, auditProcessId));
        
        return auditProcess;
    }

}
