using System.Linq.Expressions;
using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;

public class SubDepartmentValidationHelper(IRepository<Guid, SubDepartment> repository ) : ISubDepartmentValidation
{
    private readonly IRepository<Guid, SubDepartment> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    
    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(SubLocationConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<SubDepartment> ValidateSubDepartmentOwnershipAsync(Guid subDepartmentId, Guid userId)
    {
         return await OwnershipValidator.ValidateOwnershipAsync<SubDepartment, Guid>(
            _repository.Query(),
            s => s.Id == subDepartmentId,
            SubDepartmentConstants.SubDepartmentOwnerIdSelector,
            userId,
            string.Format(SubDepartmentConstants.SubDepartmentNotFoundMessage, subDepartmentId),
            string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, subDepartmentId)
        );
    }
    
    public async Task<Department> ValidateDepartmentOwnershipAsync(Guid departmentId, Guid userId)
    {
        var departmentWithSubDepartment = await OwnershipValidator.ValidateOwnershipAsync<SubDepartment, Guid>(
            _repository.Query(),
            s => s.Department.Id == departmentId,
            SubDepartmentConstants.SubDepartmentOwnerIdSelector,
            userId,
            string.Format(SubDepartmentConstants.SubDepartmentNotFoundMessage, departmentId),
            string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, departmentId)
        );
        
        return departmentWithSubDepartment.Department;
    }
}