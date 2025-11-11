using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;

public class SubDepartmentValidationHelper(
    IRepository<Guid, SubDepartment> repository,
    IRepository<Guid, Department> departmentRepository) 
    : ISubDepartmentValidation
{
    private readonly IRepository<Guid, SubDepartment> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Department> _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
    
    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(SubDepartmentConstants.UserIdEmptyMessage, nameof(userId));
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
       var department = await _departmentRepository.GetByIdAsync(departmentId) ?? throw new ArgumentException(SubDepartmentConstants.DepartmentNotFoundMessage, nameof(departmentId));
       
       var ownerId = await _departmentRepository.Query()
            .AsNoTracking()
            .Where(d => d.Id == departmentId)
            .Select(DepartmentConstants.DepartmentOwnerIdSelector)
            .FirstOrDefaultAsync();
 
       if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, departmentId)
            );
 
       return department;
    }
}