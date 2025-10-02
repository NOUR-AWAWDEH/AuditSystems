using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.Application.Constants.Processes.AuditProcessConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using Microsoft.EntityFrameworkCore;

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
       return await OwnershipValidator.ValidateOwnershipAsync<AuditProcess, Guid>(
            _repository.Query(),
            cd =>  cd.Id == auditProcessId,
            AuditProcessConstants.AuditProcessOwnerIdSelector,
            userId,
            string.Format(AuditProcessConstants.AuditProcessNotFoundMessage, auditProcessId),
            string.Format(AuditProcessConstants.UnauthorizedAccessMessage, userId, auditProcessId)
       );
    }

    public async Task<SubDepartment> ValidateSubDepartmentOwnerShipAsync(Guid subDepartmentId, Guid userId)
    {
        var subDepartmentWithAuditProcess = await OwnershipValidator.ValidateOwnershipAsync<AuditProcess, Guid>(
            _repository.Query(),
            ap => ap.SubDepartment.Id == subDepartmentId,
            AuditProcessConstants.AuditProcessOwnerIdSelector,
            userId,
            string.Format(SubDepartmentConstants.SubDepartmentNotFoundMessage, subDepartmentId),
            string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, subDepartmentId)
       );
        return subDepartmentWithAuditProcess.SubDepartment;
    }
}
