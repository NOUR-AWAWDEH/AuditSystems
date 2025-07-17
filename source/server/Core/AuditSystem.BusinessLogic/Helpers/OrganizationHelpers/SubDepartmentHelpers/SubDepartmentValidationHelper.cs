using System.Linq.Expressions;
using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.SubDepartmentConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubDepartmentHelpers;

public class SubDepartmentValidationHelper : ISubDepartmentValidation
{
    private readonly IRepository<Guid, SubDepartment> _repository;

    public SubDepartmentValidationHelper(IRepository<Guid, SubDepartment> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(SubLocationConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<SubDepartment> ValidateSubDepartmentOwnershipAsync(Guid subDepartmentId, Guid userId)
    {
        // Use the full navigation chain for SubDepartment
        var subDepartment = await _repository.GetByIdAsync(subDepartmentId, new Expression<Func<SubDepartment, object?>>[] 
        {
            s => s.Department,
            s => s.Department.SubLocation,
            s => s.Department.SubLocation.Location
        })
            ?? throw new KeyNotFoundException(string.Format(SubDepartmentConstants.SubDepartmentNotFoundMessage, subDepartmentId));
        
        if (subDepartment.Department?.SubLocation?.Location?.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, subDepartmentId));
        }
        
        return subDepartment;   
    }
    
    public async Task<Department> ValidateDepartmentOwnershipAsync(Guid departmentId, Guid userId)
    {
        // Use the generic method with includes for Department
        var department = await _repository.GetByIdAsync<Department>(departmentId, new Expression<Func<Department, object?>>[] 
        {
            d => d.SubLocation,
            d => d.SubLocation.Location
        })
            ?? throw new KeyNotFoundException(string.Format(DepartmentConstants.DepartmentNotFoundMessage, departmentId));
        
        if (department.SubLocation?.Location?.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(SubDepartmentConstants.UnauthorizedAccessMessage, userId, departmentId));
        }
        
        return department;
    }
}