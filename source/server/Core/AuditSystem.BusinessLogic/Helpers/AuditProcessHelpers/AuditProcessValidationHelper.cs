using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.Application.Constants.Processes;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;

namespace AuditSystem.BusinessLogic.Helpers.AuditProcessHelpers;

public class AuditProcessValidationHelper : IAuditProcessValidation
{
    private readonly IRepository<Guid, AuditProcess> _repository;

    public AuditProcessValidationHelper(IRepository<Guid, AuditProcess> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId) 
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(AuditProcessConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<AuditProcess> ValidateAuditProcessOwnerShipAsync(Guid auditProcessId, Guid userId)
    {
        var auditProcess = await _repository.GetByIdAsync(auditProcessId, AuditProcessConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(AuditProcessConstants.AuditProcessNotFoundMessage, auditProcessId));
        
        if (auditProcess.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(AuditProcessConstants.UnauthorizedAccessMessage, userId, auditProcessId));
        
        return auditProcess;
    }

    public async Task<SubDepartment> ValidateSubDepartmentOwnerShipAsync(Guid subDepartmentId, Guid userId)
    {
        var subDepartment = await _repository.GetByIdAsync(subDepartmentId, SubDepartmentConstants.IncludeDepartmentWithLocation)
            ?? throw new KeyNotFoundException(string.Format(SubDepartmentConstants.SubDepartmentNotFoundMessage, subDepartmentId));

        if (subDepartment.Department?.SubLocation?.Location?.UserId != userId)
            throw new ArgumentException(string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, subDepartmentId));
        
        return subDepartment;
    }
}
