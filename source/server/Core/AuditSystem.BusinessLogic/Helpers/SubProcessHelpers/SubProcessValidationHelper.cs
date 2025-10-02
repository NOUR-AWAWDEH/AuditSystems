using AuditSystem.Application.Constants.Processes.AuditProcessConst;
using AuditSystem.Application.Constants.Processes.SubProcessConst;
using AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Processes;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.SubProcessHelpers;

public class SubProcessValidationHelper(
    IRepository<Guid, SubProcess> repository,
    IRepository<Guid, AuditProcess> auditProcessRepository) 
    : ISubProcessValidation
{
    private readonly IRepository<Guid, SubProcess> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, AuditProcess> _auditProcessRepository = auditProcessRepository ?? throw new ArgumentNullException(nameof(auditProcessRepository));

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
        var auditProcess = await _auditProcessRepository.GetByIdAsync(auditProcessId)
            ?? throw new ArgumentException(string.Format(AuditProcessConstants.AuditProcessNotFoundMessage, auditProcessId), nameof(auditProcessId));

        var ownerId = await _auditProcessRepository.Query()
            .AsNoTracking()
            .Where(ap => ap.Id == auditProcessId)
            .Select(AuditProcessConstants.AuditProcessOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(AuditProcessConstants.UnauthorizedAccessMessage, userId, auditProcessId)
            );
        
        return auditProcess;
    }
}