using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.Application.Constants.Processes.AuditProcessConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;

public class AuditProcessValidationHelper(
    IRepository<Guid, AuditProcess> repository, 
    IRepository<Guid, SubDepartment> subDepartmentRepository) 
    : IAuditProcessValidation
{
    private readonly IRepository<Guid, AuditProcess> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, SubDepartment> _subDepartmentRepository = subDepartmentRepository ?? throw new ArgumentNullException(nameof(subDepartmentRepository));
    
    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(AuditProcessConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<AuditProcess> ValidateAuditProcessOwnershipAsync(Guid auditProcessId, Guid userId)
    {
       return await OwnershipValidator.ValidateOwnershipAsync<AuditProcess, Guid>(
            _repository.Query(),
            cd =>  cd.Id == auditProcessId,
            AuditProcessConstants.AuditProcessOwnerIdSelector,
            userId,
            string.Format(AuditProcessConstants.AuditProcessNotFoundMessage, auditProcessId),
            string.Format(AuditProcessConstants.UnauthorizedAccessMessage, userId, auditProcessId)
       );
    }

    public async Task<SubDepartment> ValidateSubDepartmentOwnershipAsync(Guid subDepartmentId, Guid userId)
    {
        var subDepartment = await _subDepartmentRepository.GetByIdAsync(subDepartmentId) ?? throw new KeyNotFoundException(string.Format(SubDepartmentConstants.SubDepartmentNotFoundMessage, subDepartmentId));
        var ownerId = await _subDepartmentRepository.Query()
            .Where(sd => sd.Id == subDepartmentId)
            .Select(SubDepartmentConstants.SubDepartmentOwnerIdSelector)
            .FirstOrDefaultAsync();
        
        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, subDepartmentId)
            );

        return subDepartment;
    }
}